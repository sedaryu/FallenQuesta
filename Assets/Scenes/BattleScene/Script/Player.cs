using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string Name { get; private set; }
    public float Defense { get; private set; } //��_���[�W������
    public float Speed { get; private set; } //�ړ����x
    public float Recover { get; private set; } //��b�Ԃɉ񕜂���K�b�c��
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
