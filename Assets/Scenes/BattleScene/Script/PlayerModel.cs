using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PlayerModel�N���X�ɂ��Ẳ��
//�v���p�e�B�ɑ�����ꂽ�v���C���[�̊e�X�e�[�^�X���Ǘ�����N���X
//�R���X�g���N�^�[�ɂ����āA�����Ƃ��Ď󂯎����Player�N���X�ɑ������Ă���e�X�e�[�^�X���v���p�e�B�ɑ������
//�N���X���Ɏ������ꂽ���\�b�h�ɂ����āA�e�X�e�[�^�X�̒l��ω�������

public class PlayerModel
{
    public string Name { get; private set; } //Player�̖��O
    public float Hp { get; private set; } = 1; //�̗�
    public float Defense { get; private set; } //���e������̔�_���[�W��
    public float Speed { get; private set; } //�ړ����x
    public float Recover { get; private set; } //��b�Ԃɉ񕜂���K�b�c��

    float guts = 0;
    public float Guts //�U������ۂɏ����MP�̂悤�ȃX�e�[�^�X�A���Ԍo�߂Ŏ����񕜂���
    {
        get => guts;
        //0�ȏ�1�ȉ��ɒl�����܂�悤�Ǘ�
        set
        {
            if (value < 0)
            {
                guts = 0;
            }
            else if (value > 1)
            {
                guts = 1;
            }
            else
            {
                guts = value;
            }
        }
    }

    public PlayerModel(Player player)
    { 
        Name = player.Name;
        Defense = player.Defense;
        Speed = player.Speed;
        Recover = player.Recover;
    }

    public bool DecreaseGuts(float guts) //�n���ꂽ�Ԃ�̃K�b�c������΃K�b�c�����true��Ԃ��A������Ώ����false��Ԃ�
    {
        if (Guts - guts < 0)
        {
            return false;
        }

        Guts -= guts;
        return true;
    }

    public void RecoverGuts() //���Ԍo�߂Ŏ����I��Guts���񕜂����郁�\�b�h
    {
        Guts += Recover * Time.deltaTime;
    }

    public void DecreaseHp() //Hp�����������郁�\�b�h
    {
        Hp -= Defense;
    }
}
