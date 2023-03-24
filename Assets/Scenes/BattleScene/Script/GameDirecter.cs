using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameDirecter : MonoBehaviour
{
    //�v���C���[
    public Player Player { get; private set; } //Json�ŊǗ�
    public PlayerModel PlayerModel { get; private set; }
    public GameObject PlayerObject { get; private set; }
    public PlayerPresender PlayerPresender { get; private set; }

    private PlayerGenerator playerGenerator; //�v���C���[�v���n�u����v���C���[�I�u�W�F�N�g�𐶐����邽�߂̃N���X

    //�G�l�~�[
    public Enemy Enemy { get; private set; } //Json�łŊǗ�
    public List<Enemy> Enemies { get; private set; } = new List<Enemy>();
    public List<EnemyModel> EnemyModel { get; private set; } = new List<EnemyModel>();
    public List<GameObject> EnemyObject { get; private set; } = new List<GameObject>();
    public List<EnemyPresender> EnemyPresender { get; private set; } = new List<EnemyPresender>();
    public List<Transform> EnemyTransforms { get; private set; } = new List<Transform>(); //�v���C���[�̍U���̓����蔻��Ɏg�p����

    private EnemyGenerator enemyGenerator; //�G�l�~�[�v���n�u����G�l�~�[�I�u�W�F�N�g�𐶐����邽�߂̃N���X

    //�v���W�F�N�e�B��
    public Dictionary<string, Projectile> Projectile { get; private set; } = new Dictionary<string, Projectile>(); //Json�ŊǗ�
    public Dictionary<KeyCode, Projectile> PlayerProjectile { get; private set; } = new Dictionary<KeyCode, Projectile>(); //�L�[���ƂɊ���U��

    private PlayerProjectileEvent playerProjectileEvent;
    private EnemyProjectileEvent enemyProjectileEvent;


    private void Awake()
    {
        //Json�ŊǗ�
        Projectile.Add("Knife", new Projectile("Knife", true, 2.0f, 0.2f, 0.0625f, 2.5f, 0, -0.8f));
        Projectile.Add("Fire", new Projectile("Fire", false, 0f, 0f, 3.0f, -8.5f, -90.0f, 0.8f));
        Projectile.Add("Blast", new Projectile("Blast", false, 0f, 0f, 0.5f, -8.5f, -90.0f, 0.4f));

        //Json�ŊǗ�
        Player = new Player("Sworder", 15.0f, 0.4f);
        //Enemy = new Enemy("Devil", 15.0f, 3.75f, 10, 1, 8, enemyProjectileName);

        Enemies.Add(JsonConvertToEnemy("Devil"));
        Enemies.Add(JsonConvertToEnemy("Devil"));

        PlayerProjectile.Add(KeyCode.UpArrow, Projectile["Knife"]);

        //�v���C���[�ƃG�l�~�[�̃C���X�^���X����
        GeneratePlayer();
        GenerateEnemy();

        //�C�x���g�ݒ�
        playerProjectileEvent = new PlayerProjectileEvent(PlayerPresender, EnemyPresender, PlayerProjectile, EnemyTransforms);
        PlayerObject.GetComponent<PlayerController>().UpArrowKey += playerProjectileEvent.ThrowProjectile;
        enemyProjectileEvent = new EnemyProjectileEvent(PlayerPresender, PlayerObject.GetComponent<Transform>(), Projectile);
        EnemyObject.ForEach(x => x.GetComponent<EnemyController>().ThrowProjectile += enemyProjectileEvent.ThrowProjectile);
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
        PlayerObject = playerGenerator.Generate(Player); //�X�s�[�h��X�v���C�g�Ȃǂ̃f�[�^��player���Q�Ƃ��ݒ肳���
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

    private Enemy JsonConvertToEnemy(string name)
    {
        StreamReader reader;
        reader = new StreamReader(Application.dataPath + "/JsonData/JsonEnemy" + $"/{name}.json");
        string data = reader.ReadToEnd();
        reader.Close();

        Debug.Log(data);

        JsonEnemy enemy = JsonUtility.FromJson<JsonEnemy>(data);

        Debug.Log(enemy.Name);
        Debug.Log(enemy.Projectiles);

        return new Enemy(enemy.Name, enemy.Hp, enemy.Heal, enemy.Speed, enemy.Span, enemy.Power, enemy.Projectiles);
    }
}
