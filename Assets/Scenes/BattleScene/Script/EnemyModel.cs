using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel
{
    public string Name { get; private set; }
    public float Hp { get; private set; }
    public float MaxHp { get; private set; }
    public float Heal { get; private set; }
    public float Speed { get; private set; }
    public float Power { get; private set; }
    public List<string> Projectiles { get; private set; }

    public EnemyModel(Enemy enemy)
    {
        Name = enemy.Name;
        Hp = enemy.Hp;
        MaxHp = enemy.Hp;
        Heal = enemy.Heal;
        Speed = enemy.Speed;
        Power = enemy.Power;
        Projectiles = new List<string>();
    }

    public void DecreaseHp(float damage)
    { 
        Hp -= damage;
    }

    public float SelfHealing(float deltatime)
    {
        float heal = 0;

        if (Hp <= MaxHp)
        {
            heal = Heal * (Hp / MaxHp) * deltatime;
            Hp += heal;
        }

        return heal;
    }
}
