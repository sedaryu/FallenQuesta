using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string Name { get; private set; }
    public float Speed { get; private set; } //ˆÚ“®‘¬“x
    public float Recover { get; private set; } //ˆê•bŠÔ‚É‰ñ•œ‚·‚éƒKƒbƒc—Ê
    public List<string> Projectiles { get; private set; } = new List<string>();

    public Player(string name, float speed, float recover, List<string> projectiles)
    { 
        Name = name;
        Speed = speed;
        Recover = recover;
        Projectiles = projectiles;
    }
}
