using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel
{
    public Player Player { get; private set; }

    public PlayerModel(Player player)
    { 
        Player = player;
    }
}
