using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    public float MaxHp { get; private set; }
    public float Speed { get; private set; }
    public float Span { get; private set; }
    public float Power { get; private set; }
    public List<string> Projectiles { get; private set; }

    private float span_move = 0;
    private float delta_move = 0;
    private float move = 0;

    private float span_attack = 1.0f;
    private float delta_attack = 0;
    private List<float> projectiles_attack = new List<float>();

    public event Action<EnemyController, string> ThrowProjectile;

    private SpriteRenderer spriteRenderer; //Prefabにアタッチされているスプライトレンダラーを格納
    private Sprite enemyImage; //立ち絵画像

    [System.NonSerialized] public Slider hpGauge;
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
        this.transform.position = new Vector3(0, -1, 0); //初期位置設定
        hpGauge　= this.gameObject.GetComponentInChildren<Slider>();
        hpGauge.maxValue = MaxHp;
        hpGauge.value = MaxHp;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = enemyImage;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Attack();
    }

    private void Move()
    {
        if (-9 <= this.transform.position.x + move * Time.deltaTime && this.transform.position.x + move * Time.deltaTime <= 9)
        {
            this.transform.Translate(move * Time.deltaTime, 0, 0);
        }

        delta_move += Time.deltaTime;

        if (delta_move > span_move)
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
                ThrowProjectile.Invoke(this, Projectiles[Random.Range(0, Projectiles.Count)]);
                projectiles_attack.RemoveAt(0);
            }
        }

        if (delta_attack > span_attack)
        {
            delta_attack = 0;
            projectiles_attack = GenerateProjectiles();
        }
    }

    private List<float> GenerateProjectiles()
    {
        List<float> projectiles = new List<float>();

        for (int i = 0; i < Random.Range(1, (int)Power + 1); i++)
        {
            projectiles.Add(Random.Range(0, 1.0f));
        }

        projectiles.Sort();
        return projectiles;
    }

    private void UpdateMove()
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

    public void DeadEnemy()
    { 
        this.gameObject.SetActive(false); ;
    }

    public GameObject InstanciateProjectile()
    {
        return Instantiate(ProjectilePrefab, this.transform.position, Quaternion.identity);
    }
}
