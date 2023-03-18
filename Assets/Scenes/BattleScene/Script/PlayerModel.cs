using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel
{
    public string Name { get; private set; }
    public float Hp { get; private set; } = 1; //‘Ì—Í
    public float Speed { get; private set; } //ˆÚ“®‘¬“x
    public float Guts { get; private set; } = 1;  //

    public PlayerModel(Player player)
    { 
        Name = player.Name;
        Speed = player.Speed;
    }
}
