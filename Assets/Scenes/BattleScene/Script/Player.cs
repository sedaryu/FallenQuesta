using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�v���C���[�L�����ɂ��Ẳ��

//�v���C���[�L������Hp��0�ȉ��ɂȂ�΃Q�[���I�[�o�[
//Hp�̍ő�l�͊e�L�������ʂ�1
//Defense(��_���[�W��)�̒l�ɂ���Ď󂯂�_���[�W�ʂ��ω�����
//�U�������s������Guts�������
//Guts�͎��Ԍo�߂Ŏ����񕜂���
//Projectile���Ƃɏ����Guts�͈قȂ�

public class Player
{
    public string Name { get; private set; } //Player�̖��O
    public float Defense { get; private set; } //�G�̍U�����ꔭ������ɂ��ǂꂾ��Hp���������邩�A�ŏ��l��0.1�A�ő�l��1
    public float Speed { get; private set; } //�ړ����x
    public float Recover { get; private set; } //��b�Ԃɉ񕜂���K�b�c��
    public List<string> Projectiles { get; private set; } = new List<string>(); //�g�p����Projectile��

    public Player(string name, float defense, float speed, float recover, List<string> projectiles)
    { 
        Name = name;
        Defense = defense;
        Speed = speed;
        Recover = recover;
        Projectiles = projectiles;
    }
}
