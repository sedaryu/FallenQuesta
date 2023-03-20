using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string Name { get; private set; }
    public float Speed { get; private set; } //ˆÚ“®‘¬“x
    public float Recover { get; private set; } //ˆê•bŠÔ‚É‰ñ•œ‚·‚éƒKƒbƒc—Ê

    public Player(string name, float speed, float recover)
    { 
        Name = name;
        Speed = speed;
        Recover = recover;
    }
}
