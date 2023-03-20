using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public List<Transform> EnemyTransforms { get; private set; } = new List<Transform>();

    private EnemyGenerator enemyGenerator; //エネミープレハブからエネミーオブジェクトを生成するためのクラス

    //プロジェクティル
    public Dictionary<string, Projectile> Projectile { get; private set; } = new Dictionary<string, Projectile>();
    public Dictionary<KeyCode, Projectile> PlayerProjectile { get; private set; } = new Dictionary<KeyCode, Projectile>();

    private PlayerProjectileEvent PlayerProjectileEvent;


    private void Awake()
    {
        Player = new Player("Sworder", 10.0f, 0.4f); //各プレイヤーキャラのデータはもっと適切な方法で管理する予定
        Enemy = new Enemy("Devil", 15.0f, 3.75f, 10, 1); //各エネミーキャラのデータはもっと適切な方法で管理する予定
        Projectile.Add("knife", new Projectile("knife", true, 2.0f, 0.2f, 0.0625f, 2.5f, 0, -0.8f));
        Projectile.Add("fire", new Projectile("fire", false, 0f, 0f, 3.0f, -8.5f, -90.0f, 0.8f));
        Projectile.Add("blast", new Projectile("fire", false, 0f, 0f, 0.5f, -8.5f, -90.0f, 0.8f));

        Enemies.Add(Enemy);
        Enemies.Add(Enemy);
        PlayerProjectile.Add(KeyCode.UpArrow, Projectile["knife"]);

        GeneratePlayer();
        GenerateEnemy();

        PlayerProjectileEvent = new PlayerProjectileEvent(PlayerPresender, EnemyPresender, PlayerProjectile, EnemyTransforms);
        PlayerObject.GetComponent<PlayerController>().UpArrowKey += PlayerProjectileEvent.ThrowProjectile; //イベント設定
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EnemyPresender.ForEach(x => x.SelfHealing(Time.deltaTime));
        PlayerPresender.RecoverGuts();
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
            EnemyTransforms.Add(EnemyObject[i].GetComponent<Transform>());
        }
    }
}
