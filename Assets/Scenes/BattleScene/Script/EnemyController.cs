using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    public float Hp { get; private set; }
    public float Speed { get; private set; }
    public float Span { get; private set; }

    private float span = 0;
    private float delta = 0;
    private float move = 0;

    public void Constructor(Enemy enemy)
    {
        Hp = enemy.Hp;
        Speed = enemy.Speed;
        Span = enemy.Span;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(0, -1, 0); //èâä˙à íuê›íË
    }

    // Update is called once per frame
    void Update()
    {
        if (-9 <= this.transform.position.x + move * Time.deltaTime && this.transform.position.x + move * Time.deltaTime <= 9)
        {
            this.transform.Translate(move * Time.deltaTime, 0, 0);
        }

        delta += Time.deltaTime;

        if (delta > span)
        {
            UpdateMove();
        }
    }

    private void UpdateMove()
    {
        float controller = (1 - Math.Abs(this.transform.position.x) / 9.0f);
        span = Random.Range(0, Span * controller + 0.1f);
        delta = 0;
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
}
