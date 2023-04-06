using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

//EnemyController�N���X�ɂ��Ẳ��
//�V�[����ł̃G�l�~�[�I�u�W�F�N�g�̓���A�܂�UI���Ǘ�����N���X
//�G�l�~�[�I�u�W�F�N�g��EnemyPrefab����C���X�^���X�����Ɠ����ɁAConstructor���\�b�h�����s����A
//�����Ƃ��Ď󂯎����Enemy�N���X�ɑ������Ă���e�X�e�[�^�X���v���p�e�B�ɑ������
//�����ɁA�I�u�W�F�N�g�ɃA�^�b�`����Ă���Spriterenderer��sprite�ɁA�e�L�����ŗL��sprite(�L�����摜)��������
//Start���\�b�h�ŁA�I�u�W�F�N�g�̎q�I�u�W�F�N�g�ƂȂ��Ă���UI(HpGauge)���擾����
//UI��EnemyModel�̃v���p�e�B(Hp)�̕ω��ɘA�����A�l��ω�������

public class EnemyController : MonoBehaviour
{
    public float MaxHp { get; private set; } //Hp�̍ő�l
    public float Speed { get; private set; } //�ő�ړ����x
    public float Span { get; private set; } //�ړ����x�A�ړ������̕ω��X�p��
    public float Power { get; private set; } //��b�Ԃɕ������Projectile�̐�
    public List<string> Projectiles { get; private set; } //�g�p����Projectile�̎��

    private float span_move = 0;
    private float delta_move = 0;
    private float move = 0;

    private float span_attack = 1.0f;
    private float delta_attack = 0;
    private List<float> projectiles_attack = new List<float>();

    public event Action<EnemyController, string> ThrowProjectile; //Projectile����ۂɔ�������C�x���g

    private SpriteRenderer spriteRenderer; //Prefab�ɃA�^�b�`����Ă���SpriteRenderer���i�[
    private Sprite enemyImage; //�����G�摜

    [System.NonSerialized] public Slider hpGauge; //Hp�Q�[�W��UI�I�u�W�F�N�g���i�[
    [SerializeField] private GameObject ProjectilePrefab;

    public void Constructor(Enemy enemy)
    {
        MaxHp = enemy.Hp;
        Speed = enemy.Speed;
        Span = enemy.Span;
        Power = enemy.Power;
        Projectiles = enemy.Projectiles;
        //Resources�ɕۑ����ꂽ�L�����N�^�[�摜�����[�h���A�擾����
        enemyImage = Resources.Load($"Enemy/{enemy.Name}", typeof(Sprite)) as Sprite;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = enemyImage; //Resources����擾�����L�����̉摜��SpriteRenderer�ɑ��
    }

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(0, -1, 0); //�����ʒu�ݒ�
        hpGauge�@= this.gameObject.GetComponentInChildren<Slider>(); //Hp�Q�[�W��UI���擾
        hpGauge.maxValue = MaxHp;
        hpGauge.value = MaxHp;
    }

    // Update is called once per frame
    void Update()
    {
        Move(); //�ړ�
        Attack(); //�U��
    }

    private void Move()
    {
        //��ʊO�ɂ͂���Ȃ��悤����
        if (-9 <= this.transform.position.x + move * Time.deltaTime && this.transform.position.x + move * Time.deltaTime <= 9)
        {
            this.transform.Translate(move * Time.deltaTime, 0, 0); //�ړ��𔽉f
        }

        delta_move += Time.deltaTime;

        if (delta_move > span_move) //��莞�Ԃ��o�߂�����ړ��X�s�[�h�ƈړ�������ύX
        {
            UpdateMove();
        }
    }

    private void Attack()
    {
        delta_attack += Time.deltaTime;

        if (projectiles_attack.Count != 0)
        {
            if (delta_attack >= projectiles_attack[0])
            {
                ThrowProjectile.Invoke(this, Projectiles[Random.Range(0, Projectiles.Count)]); //�w�肳�ꂽ�^�C�~���O��Projectile�����
                projectiles_attack.RemoveAt(0);
            }
        }

        if (delta_attack > span_attack) //��莞�Ԍo�߂�����������Projectile�̐��ƃ^�C�~���O��ύX
        {
            delta_attack = 0;
            projectiles_attack = GenerateProjectiles();
        }
    }

    private List<float> GenerateProjectiles() //��莞�ԓ��ɂ�����Projectile���A�ǂ̃^�C�~���O�ŕ����𗐐��Ō���
    {
        List<float> projectiles = new List<float>();

        for (int i = 0; i < Random.Range(1, (int)Power + 1); i++)
        {
            projectiles.Add(Random.Range(0, 1.0f));
        }

        projectiles.Sort();
        return projectiles;
    }

    private void UpdateMove() //�ǂꂭ�炢�̑��x�ŁA�ǂ̕����ɁA�ǂꂾ���ړ����邩�𗐐��Ō���
    {
        float controller = (1 - Math.Abs(this.transform.position.x) / 9.0f);
        span_move = Random.Range(0, Span * controller + 0.1f);
        delta_move = 0;
        if (this.transform.position.x < 0)
        {
            move = Random.Range(-Speed + Speed * controller, Speed);
        }
        else if (this.transform.position.x > 0)
        {
            move = Random.Range(-Speed, Speed + -Speed * controller);
        }
        else
        {
            move = Random.Range(-Speed, Speed);
        }
    }

    public void UpdateHpUI(float hp)
    {
        hpGauge.value = hp; //�c��Hp�ɉ�����UI���X�V
    }

    public GameObject InstanciateProjectile() //Projectile�𐶐�
    {
        return Instantiate(ProjectilePrefab, this.transform.position, Quaternion.identity);
    }
}
