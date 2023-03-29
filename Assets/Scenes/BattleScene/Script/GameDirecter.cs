using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//BattleScene�ɓo�ꂷ��L�����N�^�[�I�u�W�F�N�g�́A�v���C���[�L�����Ȃ��PlayerPrefab�A�G�l�~�[�L�����Ȃ��EnemyPrefab���C���X�^���X���邱�ƂŐ�������
//�L�������ƂɃX�e�[�^�X�̈Ⴂ���������邽�߁A�C���X�^���X�Ɠ�����Controller�X�N���v�g���A�^�b�`����
//Controller�X�N���v�g�����ŃL�������Ƃ̃X�e�[�^�X�iSpeed��Material�Ȃǁj��ύX����
//�e�L�������Ƃ̏����X�e�[�^�X�͂��ꂼ��JSON�t�@�C���ɋL�^����Ă���
//�V�[���N�����A�w�肳�ꂽJSON�t�@�C����ǂݍ��ނ��ƂŁA�e�L�����̃X�e�[�^�X��Player�N���X�AEnemy�N���X�ɑ������
//Player�N���X�EEnemy�N���X����A���ꂼ���Model��Controller�ɃX�e�[�^�X����������
//Model�͎�ɃL�����N�^�[�̓����f�[�^����������N���X
//Controller�͎�ɃL�����N�^�[�̓��o�͂���������N���X
//�U�����󂯂�A�K�b�c���񕜂���ȂǁA���炩�̒l�̕ύX���s�������ꍇ�́APresender�N���X�ŏ�������
//Presender�N���X�𗘗p���邱�Ƃɂ���āAModel��Controller�Ԃ̈ˑ�����������

public class GameDirecter : MonoBehaviour
{
    private string SelectedPlayer; //MenuScene�őI�����ꂽ�v���C���[������
    private List<string> SelectedEnemies = new List<string>(); //MenuScene�őI�����ꂽ�G�l�~�[������


    //�v���C���[
    public Player Player { get; private set; } //�v���C���[�̏����X�e�[�^�X���i�[
    public PlayerModel PlayerModel { get; private set; } //�v���C���[�̃X�e�[�^�X�ω�������
    public GameObject PlayerObject { get; private set; } //�������ꂽPlayerPrefab�������Ɋi�[
    public PlayerPresenter PlayerPresenter { get; private set; } //PlayerModel��PlayerController�Ԃ̏����̂������󂯎���

    private PlayerGenerator playerGenerator; //PlayerPrefab����v���C���[�I�u�W�F�N�g�𐶐����邽�߂̃N���X

    //�G�l�~�[
    public List<Enemy> Enemies { get; private set; } = new List<Enemy>(); //�G�l�~�[�̏����X�e�[�^�X���i�[
    public List<EnemyModel> EnemyModel { get; private set; } = new List<EnemyModel>(); //�G�l�~�[�̃X�e�[�^�X�ω�������
    public List<GameObject> EnemyObject { get; private set; } = new List<GameObject>(); //�������ꂽEnemyPrefab�������Ɋi�[
    public List<EnemyPresenter> EnemyPresenter { get; private set; } = new List<EnemyPresenter>(); //EnemyModel��EnemyController�Ԃ̏����̂������󂯎���
    public List<Transform> EnemyTransforms { get; private set; } = new List<Transform>(); //�G�l�~�[�̓����蔻��Ɏg�p����

    private EnemyGenerator enemyGenerator; //EnemyPrefab����G�l�~�[�I�u�W�F�N�g�𐶐����邽�߂̃N���X

    //�v���W�F�N�e�B��
    public Dictionary<string, Projectile> Projectiles { get; private set; } = new Dictionary<string, Projectile>(); //�t�B�[���h��Ŏg��Projectile���i�[
    public Dictionary<KeyCode, Projectile> PlayerProjectile { get; private set; } = new Dictionary<KeyCode, Projectile>(); //�L�[���ƂɊ���U��

    private PlayerProjectileEvent playerProjectileEvent; //�v���C���[��Projectile�𓊂���ۂ̃C�x���g
    private EnemyProjectileEvent enemyProjectileEvent; //�G�l�~�[��Projectile�𓊂���ۂ̃C�x���g

    //�Q�[���I�[�o�[
    private int killCount = 0; //���j�����G�̐����L�^
    private bool gameOver = false; //�Q�[���N���A�E�Q�[���I�[�o�[�̍ۂ�true�ƂȂ�

    private void Awake()
    {
        //JsonFile���Q�Ƃ��A�eProjectile�̃X�e�[�^�X���L�^
        Projectiles = JsonConvertToProjectiles();

        //MenuScene�őI�΂ꂽ�L�����N�^�[������
        SelectedPlayer = PlayerPrefs.GetString("Player");
        for (int i = 0; i < PlayerPrefs.GetInt("EnemyCount"); i++)
        {
            SelectedEnemies.Add(PlayerPrefs.GetString($"Enemies{i}"));
        }
        

        //JsonFile���Q�Ƃ��A�ePlayer�EEnemy�̃X�e�[�^�X���L�^����
        Player = JsonConvertPlayer(SelectedPlayer);
        Enemies = JsonConvertToEnemies(SelectedEnemies);

        //�ePlayer�EEnemy�̃I�u�W�F�N�g�A�R���|�[�l���g�A�X�N���v�g�𐶐�
        GeneratePlayer();
        GenerateEnemy();

        //�ePlayer�EEnemy�̃C�x���g�ݒ�
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
        EnemyPresenter.ForEach(x => x.SelfHealing(Time.deltaTime)); //�G�l�~�[��Hp���ȉ񕜏���
        PlayerPresenter.RecoverGuts(); //�v���C���[�̃K�b�c�����񕜏���

        GameOver(); //�Q�[���I�[�o�[���ǂ����𔻒�
    }

    private void GeneratePlayer()
    {
        playerGenerator = GetComponent<PlayerGenerator>(); //PlayerGenerator�X�N���v�g�͂��炩����GameDirecter�ɃA�^�b�`����Ă���
        PlayerModel = new PlayerModel(Player); //Player�̊e�����X�e�[�^�X��PlayerModel�֋L�^
        PlayerObject = playerGenerator.Generate(Player); //PlayerPrefab���C���X�^���X���APlayer��PlayerController�֊e�����X�e�[�^�X���L�^
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

    private void GameOver() //�Q�[���I�[�o�[���o
    {
        if (PlayerObject.activeInHierarchy == false && !gameOver) //Player�̃I�u�W�F�N�g����A�N�e�B�u�ɂȂ����ꍇ
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
