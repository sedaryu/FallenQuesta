using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel
{
    public string Name { get; private set; } //Player�̖��O
    public float Hp { get; private set; } = 1; //�̗�
    public float Defense { get; private set; } //���e������̔�_���[�W
    public float Speed { get; private set; } //�ړ����x
    public float Recover { get; private set; } //��b�Ԃɉ񕜂���K�b�c��

    float guts = 0;
    public float Guts
    {
        get => guts;
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

    public void RecoverGuts(float recover)
    {
        Guts += recover;
    }

    public float DecreaseHp()
    {
        Hp -= Defense;
        return Defense;
    }
}
