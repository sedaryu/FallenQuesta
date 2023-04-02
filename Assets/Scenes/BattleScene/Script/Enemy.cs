using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy
{
    public string Name { get; private set; } //Enemyの名前
    public float Hp { get; private set; } //Hp(0以下になると死亡)
    public float Heal { get; private set; } //一秒あたりのHpのオート回復量
    public float Speed { get; private set; } //移動速度
    public float Span { get; private set; } //移動速度・移動方向を変化させる時間間隔
    public float Power { get; private set; } //一秒間に放つProjectileの最大数
    public List<string> Projectiles { get; private set; } = new List<string>(); //使用するProjectileの名前

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
