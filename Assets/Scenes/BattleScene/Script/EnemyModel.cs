using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//EnemyModel�N���X�ɂ��Ẳ��
//�v���p�e�B�ɑ�����ꂽ�G�l�~�[�̊e�X�e�[�^�X���Ǘ�����N���X
//�R���X�g���N�^�[�ɂ����āA�����Ƃ��Ď󂯎����Enemy�N���X�ɑ������Ă���e�X�e�[�^�X���v���p�e�B�ɑ������

public class EnemyModel
{
    public string Name { get; private set; } //Enemy�̖��O
    public float Hp { get; private set; } //Hp(0�ȉ��ɂȂ�Ǝ��S)
    public float MaxHp { get; private set; } //�ő�Hp
    public float Heal { get; private set; } //��b�������Hp�̃I�[�g�񕜗�
    public float Speed { get; private set; } //�ړ����x
    public float Power { get; private set; } //��b�Ԃɕ���Projectile�̍ő吔
    public List<string> Projectiles { get; private set; } //�g�p����Projectile�̖��O

    public EnemyModel(Enemy enemy)
    {
        Name = enemy.Name;
        Hp = enemy.Hp;
        MaxHp = enemy.Hp;
        Heal = enemy.Heal;
        Speed = enemy.Speed;
        Power = enemy.Power;
        Projectiles = new List<string>();
    }

    public void DecreaseHp(float damage) //�_���[�W�Ԃ�Hp����������
    { 
        Hp -= damage;
    }

    public void SelfHealing() //���Ԍo�߂�Hp�������񕜂���
    {
        if (Hp <= MaxHp)
        {
            Hp += Heal * (Hp / MaxHp) * Time.deltaTime;
        }
    }
}
