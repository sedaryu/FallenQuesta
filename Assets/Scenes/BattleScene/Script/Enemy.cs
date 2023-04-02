using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy
{
    public string Name { get; private set; } //Enemy�̖��O
    public float Hp { get; private set; } //Hp(0�ȉ��ɂȂ�Ǝ��S)
    public float Heal { get; private set; } //��b�������Hp�̃I�[�g�񕜗�
    public float Speed { get; private set; } //�ړ����x
    public float Span { get; private set; } //�ړ����x�E�ړ�������ω������鎞�ԊԊu
    public float Power { get; private set; } //��b�Ԃɕ���Projectile�̍ő吔
    public List<string> Projectiles { get; private set; } = new List<string>(); //�g�p����Projectile�̖��O

    public Enemy(string name, float hp, float heal, float speed, float span, float power, List<string> projectiles)
    {
        Name = name;
        Hp = hp;
        Heal = heal;
        Speed = speed;
        Span = span;
        Power = power;
        Projectiles = projectiles;
    }
}
