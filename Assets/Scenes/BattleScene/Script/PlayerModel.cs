using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel
{
    public string Name { get; private set; }
    public float Hp { get; private set; } = 1; //‘Ì—Í
    public float Speed { get; private set; } //ˆÚ“®‘¬“x
    public float Recover { get; private set; } //ˆê•bŠÔ‚É‰ñ•œ‚·‚éƒKƒbƒc—Ê

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
        Speed = player.Speed;
        Recover = player.Recover;
    }

    public bool DecreaseGuts(float guts)
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

    public void DecreaseHp()
    {
        Hp -= 0.25f;
    }
}
