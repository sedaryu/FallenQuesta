using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    public float MaxHp { get; private set; } //Hp�̍ő�l
    public float Speed { get; private set; } //�ړ����x
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

    private SpriteRenderer spriteRenderer; //Prefab�ɃA�^�b�`����Ă���X�v���C�g�����_���[���i�[
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
        enemyImage = Resources.Load($"Enemy/{enemy.Name}", typeof(Sprite)) as Sprite;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(0, -1, 0); //�����ʒu�ݒ�
        hpGauge�@= this.gameObject.GetComponentInChildren<Slider>(); //Hp�Q�[�W��UI���擾
        hpGauge.maxValue = MaxHp;
        hpGauge.value = MaxHp;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = enemyImage; //�G�l�~�[�L�����̉摜��ݒ�
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

    public GameObject InstanciateProjectile() //Projectile�𐶐�
    {
        return Instantiate(ProjectilePrefab, this.transform.position, Quaternion.identity);
    }
}
