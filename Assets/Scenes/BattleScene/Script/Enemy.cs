using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy
{
    public string Name { get; private set; }
    public float Hp { get; private set; }
    public float Heal { get; private set; } //ˆê•b‚ ‚½‚è‚ÌHp‚ÌƒI[ƒg‰ñ•œ—Ê
    public float Speed { get; private set; }
    public float Span { get; private set; }
    public float Power { get; private set; }
    public string ProjectileString { get; private set; }
    public List<string> Projectiles { get; private set; } = new List<string>();

    public Enemy(string name, float hp, float heal, float speed, float span, float power, List<string> projectiles)
    {
        Name = name;
        Hp = hp;
        Heal = heal;
        Speed = speed;
        Span = span;
        Power = power;
        Projectiles = projectiles;
    }
}
