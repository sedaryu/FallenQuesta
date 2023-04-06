using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//GameDirecterクラスの解説
//BattleSceneロード時に、JSONファイルを参照し各キャラのステータスをPlayer/Enemyクラスに記録する
//シーンに登場する各キャラクターを管理するクラス、またオブジェクトの生成を行う
//それら生成したクラス、オブジェクトをプロパティに格納し、管理する
//各キャラクターのHpの値をPlayerModel/EnemyModelクラスを参照することで確認し、
//キャラクター死亡しているかどうかの判定を行う
//プレイヤーキャラ、エネミーキャラの死亡状況に対応し、ゲームクリア・ゲームオーバーの処理を行う

public class GameDirecter : MonoBehaviour
{
    private string SelectedPlayer; //MenuSceneで選択されたプレイヤー名を代入
    private int SelectedEnemyCount; //MenuSceneで選択されたエネミーの数を代入
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
    [SerializeField] private List<GameObject> ProjectilePrefabs = new List<GameObject>(); //各プロジェクティルのPrefabをインスペクターから代入
                                                                                          // =>インスペクターへ代入するよりも必要な物だけResourcesから
                                                                                          //Loadするように改良
    public Dictionary<string, GameObject> Projectiles { get; private set; } = new Dictionary<string, GameObject>(); //各プロジェクティルのPrefabを格納

    //攻撃イベント
    private PlayerProjectileEvent playerProjectileEvent; //プレイヤーが飛び道具を投げる際のイベントを管理
    private EnemyProjectileEvent enemyProjectileEvent; //エネミーが飛び道具を投げる際のイベントを管理

    //ゲームオーバー
    private int killCount = 0; //撃破した敵の数を記録

    private void Awake()
    {
        //各飛び道具のPrefabをProjectilesに追加
        Projectiles = SettingProjectiles();

        //MenuSceneで選ばれたキャラクター名とエネミー数を代入
        StreamReader reader = new StreamReader(Application.dataPath + "/StreamingAssets/JsonData/SelectedCharacter.json");
        string json = reader.ReadToEnd();
        reader.Close();
        SelectedCharacter selected = JsonUtility.FromJson<SelectedCharacter>(json);
        SelectedPlayer = selected.selectedPlayer;
        SelectedEnemies = selected.selectedEnemies;
        SelectedEnemyCount = selected.selectedEnemies.Count;

        //JsonFileを参照し、各プレイヤー・エネミーのステータスをPlayer・Enemyクラスに記録する
        Player = JsonConvertToPlayer(SelectedPlayer);
        Enemies = JsonConvertToEnemies(SelectedEnemies);

        //各プレイヤー・エネミーのオブジェクト、スクリプトを生成
        GeneratePlayer();
        GenerateEnemy();

        //各プレイヤー・エネミーのイベントを設定
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
        DeadJudge(); //プレイヤー・エネミーの死亡を判定

        EnemyPresenter.ForEach(x => x.SelfHealing()); //エネミーのHp自己回復処理
        PlayerPresenter.RecoverGuts(); //プレイヤーのガッツ自動回復処理
    }

    private void GeneratePlayer()
    {
        playerGenerator = GetComponent<PlayerGenerator>(); //PlayerGeneratorスクリプトはあらかじめGameDirecterにアタッチされている
        PlayerModel = new PlayerModel(Player); //Playerの各初期ステータスをPlayerModelへ代入
        PlayerObject = playerGenerator.Generate(Player); //PlayerPrefabをインスタンスし、PlayerControllerへ各ステータスを代入
        PlayerPresenter = new PlayerPresenter(PlayerModel, PlayerObject.GetComponent<PlayerController>()); //PlayerPresenterを設定
    }

    private void GenerateEnemy()
    { 
        enemyGenerator = GetComponent<EnemyGenerator>(); //EnemyGeneratorスクリプトはあらかじめGameDirecterにアタッチされている
        Enemies.ForEach(x => EnemyModel.Add(new EnemyModel(x))); //Enemyの各初期ステータスをEnemyModelへ代入
        Enemies.ForEach(x => EnemyObject.Add(enemyGenerator.Generate(x))); //EnemyPrefabをインスタンスし、EnemyControllerへ各ステータスを代入
        for (int i = 0; i < Enemies.Count; i++)
        {
            EnemyPresenter.Add(new EnemyPresenter(EnemyModel[i], EnemyObject[i].GetComponent<EnemyController>())); //PlayerPresenterを設定
            EnemyTransforms.Add(EnemyObject[i].GetComponent<Transform>()); //Transformのリストを設定
        }
    }

    private Player JsonConvertToPlayer(string name)
    {
        StreamReader reader;
        reader = new StreamReader(Application.dataPath + "/StreamingAssets/JsonData/JsonPlayer" + $"/{name}.json");
        string data = reader.ReadToEnd();
        reader.Close();
        JsonPlayer player = JsonUtility.FromJson<JsonPlayer>(data); //JsonPlayerクラスに各ステータスの値を代入
        return new Player(player.Name, player.Defense, player.Speed, player.Recover, player.Projectiles); //Playerクラスを生成すると同時にステータスを受け取る
    }

    private List<Enemy> JsonConvertToEnemies(List<string> name)
    {
        List<Enemy> enemies = new List<Enemy>();
        StreamReader reader;

        for (int i = 0; i < name.Count; i++)
        {
            reader = new StreamReader(Application.dataPath + "/StreamingAssets/JsonData/JsonEnemy" + $"/{name[i]}.json");
            string data = reader.ReadToEnd();
            reader.Close();
            JsonEnemy enemy = JsonUtility.FromJson<JsonEnemy>(data); //JsonEnemyクラスに各ステータスの値を代入
            //Enemyクラスを生成すると同時にステータスを受け取る
            enemies.Add(new Enemy(enemy.Name, enemy.Hp, enemy.Heal, enemy.Speed, enemy.Span, enemy.Power, enemy.Projectiles));
        }

        return enemies;
    }

    private Dictionary<string, GameObject> SettingProjectiles() //インスペクタから代入したプロジェクティルのPrefabの名前をキーとしてDictionaryに代入
    {
        Dictionary<string, GameObject> projectiles = new Dictionary<string, GameObject>();
        
        ProjectilePrefabs.ForEach(x => projectiles.Add(x.GetComponent<ProjectileController>().Name, x));

        return projectiles;
    }

    private void DeadJudge() //死亡判定
    {
        //プレイヤーの死亡判定
        if (PlayerModel.Hp <= 0) //プレイヤーのHpが0以下の場合
        {
            PlayerObject.SetActive(false); //非アクティブ化
            PlayerObject.GetComponent<Transform>().position = new Vector3(500, 0, 0); //エネミーの飛び道具が当たらない場所まで当たり判定(Transform)を移動
            GameOver(); //ゲームオーバー演出を実行
        }

        //エネミーの死亡判定
        if (EnemyModel.Count(x => x.Hp <= 0) > 0) //Hpが0以下のエネミーが一つでもある場合
        {
            for (int i = 0; i < EnemyModel.Count(x => x.Hp <= 0); i++)
            {
                int index = EnemyModel.FindIndex(x => x.Hp <= 0); //撃破したエネミーのインデックスを取得
                //エネミーに関連するオブジェクト・スクリプトを格納先から除外する
                EnemyPresenter.RemoveAt(index);
                EnemyModel.RemoveAt(index);
                EnemyTransforms.RemoveAt(index);
                Destroy(EnemyObject[index]);
                EnemyObject.RemoveAt(index);
                
                killCount++; //撃破カウントを更新
            }

            if (killCount == SelectedEnemyCount) //エネミー撃破数がエネミー数に達した場合
            {
                GameClear(); //ステージクリアー演出を実行
            }
        }
    }

    private void GameOver() //ゲームオーバー演出
    {
        GameObject.Find("background_beach").GetComponent<SpriteRenderer>().color = Color.red; //背景を赤に
        StartCoroutine(BackToMenu()); //MenuSceneへ戻る
    }

    private void GameClear()
    {
        GameObject.Find("Canvas").transform.Find("FieldConquested").gameObject.SetActive(true); //テキストを表示
        StartCoroutine(BackToMenu()); //MenuSceneへ戻る
    }

    IEnumerator BackToMenu()
    {
        yield return new WaitForSeconds(7.5f);
        SceneManager.LoadScene("MenuScene");
    }
}
