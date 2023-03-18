using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirecter : MonoBehaviour
{
    //プレイヤー
    public Player Player { get; private set; } //未実装
    public PlayerModel PlayerModel { get; private set; }
    public GameObject PlayerObject { get; private set; }
    public PlayerPresender PlayerPresender { get; private set; }

    private PlayerGenerator playerGenerator; //プレイヤープレハブからプレイヤーオブジェクトを生成するためのクラス

    //エネミー
    public Enemy Enemy { get; private set; } //未実装
    public List<Enemy> Enemies { get; private set; } = new List<Enemy>();
    public List<EnemyModel> EnemyModel { get; private set; } = new List<EnemyModel>();
    public List<GameObject> EnemyObject { get; private set; } = new List<GameObject>();
    public List<EnemyPresender> EnemyPresender { get; private set; } = new List<EnemyPresender>();

    private EnemyGenerator enemyGenerator; //エネミープレハブからエネミーオブジェクトを生成するためのクラス

    private void Awake()
    {
        Player = new Player("Sworder", 10.0f); //各プレイヤーキャラのデータはもっと適切な方法で管理する予定
        Enemy = new Enemy("Devil", 15.0f, 10, 1); //各エネミーキャラのデータはもっと適切な方法で管理する予定
        Enemies.Add(Enemy);
        Enemies.Add(Enemy);
        GeneratePlayer();
        GenerateEnemy();

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GeneratePlayer()
    {
        playerGenerator = GetComponent<PlayerGenerator>();
        PlayerModel = new PlayerModel(Player); //
        PlayerObject = playerGenerator.Generate(Player); //スピードやスプライトなどのデータをplayerを参照し設定される
        PlayerPresender = new PlayerPresender(PlayerModel, PlayerObject.GetComponent<PlayerController>());
    }

    private void GenerateEnemy()
    { 
        enemyGenerator = GetComponent<EnemyGenerator>();
        Enemies.ForEach(x => EnemyModel.Add(new EnemyModel(x)));
        Enemies.ForEach(x => EnemyObject.Add(enemyGenerator.Generate(x)));
        for (int i = 0; i < Enemies.Count; i++)
        {
            EnemyPresender.Add(new EnemyPresender(EnemyModel[i], EnemyObject[i].GetComponent<EnemyController>()));
        }
    }
}
