using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile
{
    public string Name { get; private set; }
    public bool Player { get; private set; }
    public float Attack { get; private set; }
    public float Cost { get; private set; }
    public float FlyingTime { get; private set; }
    public float Speed { get; private set; }
    public float Rotation { get; private set; }
    public float Scale { get; private set; }

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
