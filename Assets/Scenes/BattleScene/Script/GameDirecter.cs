using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEngine;
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
    [SerializeField] private string PlayerSelect;
    [SerializeField] private List<string> EnemySelect;


    //プレイヤー
    public Player Player { get; private set; }
    public PlayerModel PlayerModel { get; private set; }
    public GameObject PlayerObject { get; private set; } //生成されたPlayerPrefabをここに格納
    public PlayerPresenter PlayerPresenter { get; private set; }

    private PlayerGenerator playerGenerator; //PlayerPrefabからプレイヤーオブジェクトを生成するためのクラス

    //エネミー
    public List<Enemy> Enemies { get; private set; } = new List<Enemy>();
    public List<EnemyModel> EnemyModel { get; private set; } = new List<EnemyModel>();
    public List<GameObject> EnemyObject { get; private set; } = new List<GameObject>(); //生成されたEnemyPrefabをここに格納
    public List<EnemyPresenter> EnemyPresenter { get; private set; } = new List<EnemyPresenter>();
    public List<Transform> EnemyTransforms { get; private set; } = new List<Transform>(); //プレイヤーの攻撃の当たり判定に使用する

    private EnemyGenerator enemyGenerator; //EnemyPrefabからエネミーオブジェクトを生成するためのクラス

    //プロジェクティル
    public Dictionary<string, Projectile> Projectiles { get; private set; } = new Dictionary<string, Projectile>(); //Jsonで管理
    public Dictionary<KeyCode, Projectile> PlayerProjectile { get; private set; } = new Dictionary<KeyCode, Projectile>(); //キーごとに割り振る

    private PlayerProjectileEvent playerProjectileEvent;
    private EnemyProjectileEvent enemyProjectileEvent;


    private void Awake()
    {
        //Jsonで管理
        Projectiles = JsonConvertToProjectiles();

        //Jsonで管理
        Player = JsonConvertPlayer(PlayerSelect);
        Enemies = JsonConvertToEnemies(EnemySelect);

        //プレイヤーとエネミーの各オブジェクト、コンポーネント、スクリプト生成
        GeneratePlayer();
        GenerateEnemy();

        //イベント設定
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
        EnemyPresenter.ForEach(x => x.SelfHealing(Time.deltaTime));
        PlayerPresenter.RecoverGuts();
    }

    private void GeneratePlayer()
    {
        playerGenerator = GetComponent<PlayerGenerator>();
        PlayerModel = new PlayerModel(Player);
        PlayerObject = playerGenerator.Generate(Player);
        PlayerPresenter = new PlayerPresenter(PlayerModel, PlayerObject.GetComponent<PlayerController>());
    }

    private void GenerateEnemy()
    { 
        enemyGenerator = GetComponent<EnemyGenerator>();
        Enemies.ForEach(x => EnemyModel.Add(new EnemyModel(x)));
        Enemies.ForEach(x => EnemyObject.Add(enemyGenerator.Generate(x)));
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
        return new Player(player.Name, player.Speed, player.Recover, player.Projectiles);
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
}
