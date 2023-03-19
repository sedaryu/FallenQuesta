using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel
{
    public string Name { get; private set; }
    public float Hp { get; private set; } = 1; //体力
    public float Speed { get; private set; } //移動速度
    public float Guts { get; private set; } = 1;  //

    public PlayerModel(Player player)
    { 
        Name = player.Name;
        Speed = player.Speed;
    }

    public void DecreaseGuts(float guts)
    {
        Guts -= guts;
    }
}
