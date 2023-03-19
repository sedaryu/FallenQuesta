using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel
{
    public string Name { get; private set; }
    public float Hp { get; private set; }
    public float Speed { get; private set; }

    public EnemyModel(Enemy enemy)
    {
        Name = enemy.Name;
        Hp = enemy.Hp;
        Speed = enemy.Speed;
    }

    public void DecreaseHp(float damage)
    { 
        Hp -= damage;
    }
}
