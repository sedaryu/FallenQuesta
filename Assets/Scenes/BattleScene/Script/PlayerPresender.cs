using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPresender
{
    private PlayerModel PlayerModel { get; set; }
    private PlayerController PlayerController { get; set; }

    public PlayerPresender(PlayerModel playerModel, PlayerController playerController)
    {
        PlayerModel = playerModel;
        PlayerController = playerController;
    }

    public void DecreaseGuts(float guts)
    {
        PlayerModel.DecreaseGuts(guts);
    }
}
