using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    public string Name { get; private set; }
    public float Hp { get; private set; }
    public float Speed { get; private set; }
    public float Span { get; private set; }

    public Enemy(string name, float hp, float speed, float span)
    {
        Name = name;
        Hp = hp;
        Speed = speed;
        Span = span;
    }
}
