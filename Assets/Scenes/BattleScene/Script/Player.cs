using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string Name { get; private set; }
    public float Speed { get; private set; } //�ړ����x

    public Player(string name, float speed)
    { 
        Name = name;
        Speed = speed;
    }
}
