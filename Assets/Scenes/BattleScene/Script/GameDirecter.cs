using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//BattleSceneに登場するキャラクターオブジェクトは、プレイヤーキャラならばPlayerPrefab、エネミーキャラならばEnemyPrefabをインスタンスすることで生成する
//キャラごとにステータスの違いを持たせるため、インスタンスと同時にControllerスクリプトをアタッチする
//Controllerスクリプト内部でキャラごとのステータス（SpeedやMaterialなど）を変更する
//各キャラごとの初期ステータスはそれぞれJSONファイルに記録されている
//シーン起動時、指定されたJSONファイルを読み込むことで、各キャラのステータスをPlayerクラス、Enemyクラスに代入する
//Playerクラス・Enemyクラスから、それぞれのModelとControllerにステータスが代入される
//Modelは主にキャラクターの内部データを処理するクラス
//Controllerは主にキャラクターの入出力を処理するクラス
//攻撃を受ける、ガッツが回復するなど、何らかの値の変更を行いたい場合は、Presenderクラスで処理する
//Presenderクラスを利用することによって、ModelとController間の依存性を下げる

public class GameDirecter : MonoBehaviour
{
    private string SelectedPlayer; //MenuSceneで選択されたプレイヤー名を代入
    private List<string> SelectedEnemies = new List<string>(); //MenuSceneで選択されたエネミー名を代入


    //プレイヤー
    public Player Player { get; private set; } //プレイヤーの初期ステータスを格納
    public PlayerModel PlayerModel { get; private set; } //プレイヤーのステータス変化を処理
    public GameObject PlayerObject { get; private set; } //生成されたPlayerPrefabをここに格納
    public PlayerPresenter PlayerPresenter { get; private set; } //PlayerModelとPlayerController間の処理のやり取りを受け持つ

    private PlayerGenerator playerGenerator; //PlayerPrefabからプレイヤーオブジェクトを生成するためのクラス

    //エネミー
    public List<Enemy> Enemies { get; private set; } = new List<Enemy>(); //エネミーの初期ステータスを格納
    public List<EnemyModel> EnemyModel { get; private set; } = new List<EnemyModel>(); //エネミーのステータス変化を処理
    public List<GameObject> EnemyObject { get; private set; } = new List<GameObject>(); //生成されたEnemyPrefabをここに格納
    public List<EnemyPresenter> EnemyPresenter { get; private set; } = new List<EnemyPresenter>(); //EnemyModelとEnemyController間の処理のやり取りを受け持つ
    public List<Transform> EnemyTransforms { get; private set; } = new List<Transform>(); //エネミーの当たり判定に使用する

    private EnemyGenerator enemyGenerator; //EnemyPrefabからエネミーオブジェクトを生成するためのクラス

    //プロジェクティル
    public Dictionary<string, Projectile> Projectiles { get; private set; } = new Dictionary<string, Projectile>(); //フィールド上で使うProjectileを格納
    public Dictionary<KeyCode, Projectile> PlayerProjectile { get; private set; } = new Dictionary<KeyCode, Projectile>(); //キーごとに割り振る

    private PlayerProjectileEvent playerProjectileEvent; //プレイヤーがProjectileを投げる際のイベント
    private EnemyProjectileEvent enemyProjectileEvent; //エネミーがProjectileを投げる際のイベント

    //ゲームオーバー
    private int killCount = 0; //撃破した敵の数を記録
    private bool gameOver = false; //ゲームクリア・ゲームオーバーの際にtrueとなる

    private void Awake()
    {
        //JsonFileを参照し、各Projectileのステータスを記録
        Projectiles = JsonConvertToProjectiles();

        //MenuSceneで選ばれたキャラクター名を代入
        SelectedPlayer = PlayerPrefs.GetString("Player");
        for (int i = 0; i < PlayerPrefs.GetInt("EnemyCount"); i++)
        {
            SelectedEnemies.Add(PlayerPrefs.GetString($"Enemies{i}"));
        }
        

        //JsonFileを参照し、各Player・Enemyのステータスを記録する
        Player = JsonConvertPlayer(SelectedPlayer);
        Enemies = JsonConvertToEnemies(SelectedEnemies);

        //各Player・Enemyのオブジェクト、コンポーネント、スクリプトを生成
        GeneratePlayer();
        GenerateEnemy();

        //各Player・Enemyのイベント設定
        playerProjectileEvent = new PlayerProjectileEvent(PlayerPresenter, EnemyPresenter, Projectiles, EnemyTransforms);
        PlayerObject.GetComponent<PlayerController>().UpArrowKey += playerProjectileEvent.ThrowProjectile;
        enemyProjectileEvent = new EnemyProjectileEvent(PlayerPresenter, PlayerObject.GetComponent<Transform>(), Projectiles);
        EnemyObject.ForEach(x => x.GetComponent<EnemyController>().ThrowProjectile += enemyProjectileEvent.ThrowProjectile);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        EnemyPresenter.ForEach(x => x.SelfHealing(Time.deltaTime)); //エネミーのHp自己回復処理
        PlayerPresenter.RecoverGuts(); //プレイヤーのガッツ自動回復処理

        GameOver(); //ゲームオーバーかどうかを判定
    }

    private void GeneratePlayer()
    {
        playerGenerator = GetComponent<PlayerGenerator>(); //PlayerGeneratorスクリプトはあらかじめGameDirecterにアタッチされている
        PlayerModel = new PlayerModel(Player); //Playerの各初期ステータスをPlayerModelへ記録
        PlayerObject = playerGenerator.Generate(Player); //PlayerPrefabをインスタンスし、PlayerのPlayerControllerへ各初期ステータスを記録
        PlayerPresenter = new PlayerPresenter(PlayerModel, PlayerObject.GetComponent<PlayerController>());
    }

    private void GenerateEnemy()
    { 
        enemyGenerator = GetComponent<EnemyGenerator>();
        Enemies.ForEach(x => EnemyModel.Add(new EnemyModel(x)));
        Enemies.ForEach(x => EnemyObject.Add(enemyGenerator.Generate(x)));
        EnemyObject.ForEach(x => x.GetComponent<EnemyController>().EnemyDead += KillEnemy);
        for (int i = 0; i < Enemies.Count; i++)
        {
            EnemyPresenter.Add(new EnemyPresenter(EnemyModel[i], EnemyObject[i].GetComponent<EnemyController>()));
            EnemyTransforms.Add(EnemyObject[i].GetComponent<Transform>());
        }
    }

    private Player JsonConvertPlayer(string name)
    {
        StreamReader reader;
        reader = new StreamReader(Application.dataPath + "/JsonData/JsonPlayer" + $"/{name}.json");
        string data = reader.ReadToEnd();
        reader.Close();
        JsonPlayer player = JsonUtility.FromJson<JsonPlayer>(data);
        return new Player(player.Name, player.Defense, player.Speed, player.Recover, player.Projectiles);
    }

    private List<Enemy> JsonConvertToEnemies(List<string> name)
    {
        List<Enemy> enemies = new List<Enemy>();
        StreamReader reader;

        for (int i = 0; i < name.Count; i++)
        {
            reader = new StreamReader(Application.dataPath + "/JsonData/JsonEnemy" + $"/{name[i]}.json");
            string data = reader.ReadToEnd();
            reader.Close();
            JsonEnemy enemy = JsonUtility.FromJson<JsonEnemy>(data);
            enemies.Add(new Enemy(enemy.Name, enemy.Hp, enemy.Heal, enemy.Speed, enemy.Span, enemy.Power, enemy.Projectiles));
        }

        return enemies;
    }

    private Dictionary<string, Projectile> JsonConvertToProjectiles()
    {
        Dictionary<string, Projectile> projectiles = new Dictionary<string, Projectile>();

        StreamReader reader;
        for (int i = 0; i < 3; i++)
        {
            reader = new StreamReader(Application.dataPath + "/JsonData/JsonProjectile" + $"/{i}.json");
            string data = reader.ReadToEnd();
            reader.Close();
            JsonProjectile projectile = JsonUtility.FromJson<JsonProjectile>(data);
            projectiles.Add(projectile.Name, new Projectile(projectile.Name, projectile.Player, projectile.Attack, projectile.Cost, 
                                                            projectile.FlyingTime, projectile.Speed, projectile.Rotation, projectile.Scale));
        }

        return projectiles;
    }

    private void GameOver() //ゲームオーバー演出
    {
        if (PlayerObject.activeInHierarchy == false && !gameOver) //Playerのオブジェクトが非アクティブになった場合
        {
            gameOver = true;
            GameObject.Find("background_beach").GetComponent<SpriteRenderer>().color = Color.red;
            StartCoroutine(BackToMenu());
        }

        if (killCount == Enemies.Count && !gameOver)
        {
            gameOver = true;
            GameObject.Find("Canvas").transform.Find("FieldConquested").gameObject.SetActive(true);
            StartCoroutine(BackToMenu());
        }
    }

    public void KillEnemy()
    { 
        killCount++;
    }

    IEnumerator BackToMenu()
    {
        yield return new WaitForSeconds(7.5f);
        SceneManager.LoadScene("MenuScene");
    }
}
