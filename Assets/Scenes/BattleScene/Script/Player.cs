using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string Name { get; private set; }
    public float Defense { get; private set; } //被ダメージ減少率
    public float Speed { get; private set; } //移動速度
    public float Recover { get; private set; } //一秒間に回復するガッツ量
    public List<string> Projectiles { get; private set; } = new List<string>();

    public Player(string name, float defense, float speed, float recover, List<string> projectiles)
    { 
        Name = name;
        Defense = defense;
        Speed = speed;
        Recover = recover;
        Projectiles = projectiles;
    }
}
