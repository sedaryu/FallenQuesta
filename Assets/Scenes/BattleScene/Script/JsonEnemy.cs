using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JsonEnemy
{
    public string Name;
    public float Hp;
    public float Heal;
    public float Speed;
    public float Span;
    public float Power;
    public List<string> Projectiles;
}
