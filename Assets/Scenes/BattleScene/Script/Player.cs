using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string Name { get; private set; }
    public float Speed { get; private set; } //�ړ����x
    public float Recover { get; private set; } //��b�Ԃɉ񕜂���K�b�c��

    public Player(string name, float speed, float recover)
    { 
        Name = name;
        Speed = speed;
        Recover = recover;
    }
}
