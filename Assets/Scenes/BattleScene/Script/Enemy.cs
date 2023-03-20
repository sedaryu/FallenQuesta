using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    public string Name { get; private set; }
    public float Hp { get; private set; }
    public float Heal { get; private set; } //ˆê•b‚ ‚½‚è‚ÌHp‚ÌƒI[ƒg‰ñ•œ—Ê
    public float Speed { get; private set; }
    public float Span { get; private set; }
    public List<Projectile> Projectiles { get; private set; }

    public Enemy(string name, float hp, float heal, float speed, float span)
    {
        Name = name;
        Hp = hp;
        Heal = heal;
        Speed = speed;
        Span = span;
    }
}
