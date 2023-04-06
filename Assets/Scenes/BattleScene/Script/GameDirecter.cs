using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//GameDirecter�N���X�̉��
//BattleScene���[�h���ɁAJSON�t�@�C�����Q�Ƃ��e�L�����̃X�e�[�^�X��Player/Enemy�N���X�ɋL�^����
//�V�[���ɓo�ꂷ��e�L�����N�^�[���Ǘ�����N���X�A�܂��I�u�W�F�N�g�̐������s��
//����琶�������N���X�A�I�u�W�F�N�g���v���p�e�B�Ɋi�[���A�Ǘ�����
//�e�L�����N�^�[��Hp�̒l��PlayerModel/EnemyModel�N���X���Q�Ƃ��邱�ƂŊm�F���A
//�L�����N�^�[���S���Ă��邩�ǂ����̔�����s��
//�v���C���[�L�����A�G�l�~�[�L�����̎��S�󋵂ɑΉ����A�Q�[���N���A�E�Q�[���I�[�o�[�̏������s��

public class GameDirecter : MonoBehaviour
{
    private string SelectedPlayer; //MenuScene�őI�����ꂽ�v���C���[������
    private int SelectedEnemyCount; //MenuScene�őI�����ꂽ�G�l�~�[�̐�����
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
    [SerializeField] private List<GameObject> ProjectilePrefabs = new List<GameObject>(); //�e�v���W�F�N�e�B����Prefab���C���X�y�N�^�[������
                                                                                          // =>�C���X�y�N�^�[�֑����������K�v�ȕ�����Resources����
                                                                                          //Load����悤�ɉ���
    public Dictionary<string, GameObject> Projectiles { get; private set; } = new Dictionary<string, GameObject>(); //�e�v���W�F�N�e�B����Prefab���i�[

    //�U���C�x���g
    private PlayerProjectileEvent playerProjectileEvent; //�v���C���[����ѓ���𓊂���ۂ̃C�x���g���Ǘ�
    private EnemyProjectileEvent enemyProjectileEvent; //�G�l�~�[����ѓ���𓊂���ۂ̃C�x���g���Ǘ�

    //�Q�[���I�[�o�[
    private int killCount = 0; //���j�����G�̐����L�^

    private void Awake()
    {
        //�e��ѓ����Prefab��Projectiles�ɒǉ�
        Projectiles = SettingProjectiles();

        //MenuScene�őI�΂ꂽ�L�����N�^�[���ƃG�l�~�[������
        StreamReader reader = new StreamReader(Application.dataPath + "/StreamingAssets/JsonData/SelectedCharacter.json");
        string json = reader.ReadToEnd();
        reader.Close();
        SelectedCharacter selected = JsonUtility.FromJson<SelectedCharacter>(json);
        SelectedPlayer = selected.selectedPlayer;
        SelectedEnemies = selected.selectedEnemies;
        SelectedEnemyCount = selected.selectedEnemies.Count;

        //JsonFile���Q�Ƃ��A�e�v���C���[�E�G�l�~�[�̃X�e�[�^�X��Player�EEnemy�N���X�ɋL�^����
        Player = JsonConvertToPlayer(SelectedPlayer);
        Enemies = JsonConvertToEnemies(SelectedEnemies);

        //�e�v���C���[�E�G�l�~�[�̃I�u�W�F�N�g�A�X�N���v�g�𐶐�
        GeneratePlayer();
        GenerateEnemy();

        //�e�v���C���[�E�G�l�~�[�̃C�x���g��ݒ�
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
        DeadJudge(); //�v���C���[�E�G�l�~�[�̎��S�𔻒�

        EnemyPresenter.ForEach(x => x.SelfHealing()); //�G�l�~�[��Hp���ȉ񕜏���
        PlayerPresenter.RecoverGuts(); //�v���C���[�̃K�b�c�����񕜏���
    }

    private void GeneratePlayer()
    {
        playerGenerator = GetComponent<PlayerGenerator>(); //PlayerGenerator�X�N���v�g�͂��炩����GameDirecter�ɃA�^�b�`����Ă���
        PlayerModel = new PlayerModel(Player); //Player�̊e�����X�e�[�^�X��PlayerModel�֑��
        PlayerObject = playerGenerator.Generate(Player); //PlayerPrefab���C���X�^���X���APlayerController�֊e�X�e�[�^�X����
        PlayerPresenter = new PlayerPresenter(PlayerModel, PlayerObject.GetComponent<PlayerController>()); //PlayerPresenter��ݒ�
    }

    private void GenerateEnemy()
    { 
        enemyGenerator = GetComponent<EnemyGenerator>(); //EnemyGenerator�X�N���v�g�͂��炩����GameDirecter�ɃA�^�b�`����Ă���
        Enemies.ForEach(x => EnemyModel.Add(new EnemyModel(x))); //Enemy�̊e�����X�e�[�^�X��EnemyModel�֑��
        Enemies.ForEach(x => EnemyObject.Add(enemyGenerator.Generate(x))); //EnemyPrefab���C���X�^���X���AEnemyController�֊e�X�e�[�^�X����
        for (int i = 0; i < Enemies.Count; i++)
        {
            EnemyPresenter.Add(new EnemyPresenter(EnemyModel[i], EnemyObject[i].GetComponent<EnemyController>())); //PlayerPresenter��ݒ�
            EnemyTransforms.Add(EnemyObject[i].GetComponent<Transform>()); //Transform�̃��X�g��ݒ�
        }
    }

    private Player JsonConvertToPlayer(string name)
    {
        StreamReader reader;
        reader = new StreamReader(Application.dataPath + "/StreamingAssets/JsonData/JsonPlayer" + $"/{name}.json");
        string data = reader.ReadToEnd();
        reader.Close();
        JsonPlayer player = JsonUtility.FromJson<JsonPlayer>(data); //JsonPlayer�N���X�Ɋe�X�e�[�^�X�̒l����
        return new Player(player.Name, player.Defense, player.Speed, player.Recover, player.Projectiles); //Player�N���X�𐶐�����Ɠ����ɃX�e�[�^�X���󂯎��
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
            JsonEnemy enemy = JsonUtility.FromJson<JsonEnemy>(data); //JsonEnemy�N���X�Ɋe�X�e�[�^�X�̒l����
            //Enemy�N���X�𐶐�����Ɠ����ɃX�e�[�^�X���󂯎��
            enemies.Add(new Enemy(enemy.Name, enemy.Hp, enemy.Heal, enemy.Speed, enemy.Span, enemy.Power, enemy.Projectiles));
        }

        return enemies;
    }

    private Dictionary<string, GameObject> SettingProjectiles() //�C���X�y�N�^�����������v���W�F�N�e�B����Prefab�̖��O���L�[�Ƃ���Dictionary�ɑ��
    {
        Dictionary<string, GameObject> projectiles = new Dictionary<string, GameObject>();
        
        ProjectilePrefabs.ForEach(x => projectiles.Add(x.GetComponent<ProjectileController>().Name, x));

        return projectiles;
    }

    private void DeadJudge() //���S����
    {
        //�v���C���[�̎��S����
        if (PlayerModel.Hp <= 0) //�v���C���[��Hp��0�ȉ��̏ꍇ
        {
            PlayerObject.SetActive(false); //��A�N�e�B�u��
            PlayerObject.GetComponent<Transform>().position = new Vector3(500, 0, 0); //�G�l�~�[�̔�ѓ��������Ȃ��ꏊ�܂œ����蔻��(Transform)���ړ�
            GameOver(); //�Q�[���I�[�o�[���o�����s
        }

        //�G�l�~�[�̎��S����
        if (EnemyModel.Count(x => x.Hp <= 0) > 0) //Hp��0�ȉ��̃G�l�~�[����ł�����ꍇ
        {
            for (int i = 0; i < EnemyModel.Count(x => x.Hp <= 0); i++)
            {
                int index = EnemyModel.FindIndex(x => x.Hp <= 0); //���j�����G�l�~�[�̃C���f�b�N�X���擾
                //�G�l�~�[�Ɋ֘A����I�u�W�F�N�g�E�X�N���v�g���i�[�悩�珜�O����
                EnemyPresenter.RemoveAt(index);
                EnemyModel.RemoveAt(index);
                EnemyTransforms.RemoveAt(index);
                Destroy(EnemyObject[index]);
                EnemyObject.RemoveAt(index);
                
                killCount++; //���j�J�E���g���X�V
            }

            if (killCount == SelectedEnemyCount) //�G�l�~�[���j�����G�l�~�[���ɒB�����ꍇ
            {
                GameClear(); //�X�e�[�W�N���A�[���o�����s
            }
        }
    }

    private void GameOver() //�Q�[���I�[�o�[���o
    {
        GameObject.Find("background_beach").GetComponent<SpriteRenderer>().color = Color.red; //�w�i��Ԃ�
        StartCoroutine(BackToMenu()); //MenuScene�֖߂�
    }

    private void GameClear()
    {
        GameObject.Find("Canvas").transform.Find("FieldConquested").gameObject.SetActive(true); //�e�L�X�g��\��
        StartCoroutine(BackToMenu()); //MenuScene�֖߂�
    }

    IEnumerator BackToMenu()
    {
        yield return new WaitForSeconds(7.5f);
        SceneManager.LoadScene("MenuScene");
    }
}
