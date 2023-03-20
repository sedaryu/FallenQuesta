using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string Name { get; private set; }
    public float Speed { get; private set; } //移動速度
    public float Recover { get; private set; } //一秒間に回復するガッツ量

    public Player(string name, float speed, float recover)
    { 
        Name = name;
        Speed = speed;
        Recover = recover;
    }
}
