using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile
{
    public string Name { get; private set; } //Projectile�̖��O
    public bool Player { get; private set; } //�v���C���[����������Projectile�Ȃ��true�A�G�l�~�[����Ȃ��false
    public float Attack { get; private set; } //Projectile�����������ہA�^����_���[�W��
    public float Cost { get; private set; } //Projectile����ہA�����K�b�c��
    public float FlyingTime { get; private set; } //Projectile�ɓ����蔻�肪���܂ł̎���
    public float Speed { get; private set; } //Projectile�̈ړ����x
    public float Rotation { get; private set; } //Projectile�̉�]���x
    public float Scale { get; private set; } //Projectile�̊g�呬�x

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
