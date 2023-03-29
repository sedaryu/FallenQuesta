using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile
{
    public string Name { get; private set; } //Projectileの名前
    public bool Player { get; private set; } //プレイヤーから放たれるProjectileならばtrue、エネミーからならばfalse
    public float Attack { get; private set; } //Projectileが当たった際、与えるダメージ量
    public float Cost { get; private set; } //Projectileを放つ際、消費するガッツ量
    public float FlyingTime { get; private set; } //Projectileに当たり判定がつくまでの時間
    public float Speed { get; private set; } //Projectileの移動速度
    public float Rotation { get; private set; } //Projectileの回転速度
    public float Scale { get; private set; } //Projectileの拡大速度

    public Projectile(string name, bool player, float attack, float cost, float flyingTime, float speed, float rotation, float scale)
    {
        Name = name;
        Player = player;
        Attack = attack;
        Cost = cost;
        FlyingTime = flyingTime;
        Speed = speed;
        Rotation = rotation;
        Scale = scale;
    }
}
