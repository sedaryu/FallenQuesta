using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel
{
    public string Name { get; private set; } //Playerの名前
    public float Hp { get; private set; } = 1; //体力
    public float Defense { get; private set; } //一被弾あたりの被ダメージ
    public float Speed { get; private set; } //移動速度
    public float Recover { get; private set; } //一秒間に回復するガッツ量

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

    public bool DecreaseGuts(float guts) //渡されたぶんのガッツがあればガッツを消費しtrueを返す、無ければ消費せずfalseを返す
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
