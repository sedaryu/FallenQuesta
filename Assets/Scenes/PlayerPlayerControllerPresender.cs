using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlayerControllerPresender
{
    private PlayerModel PlayerModel { get; set; }
    private PlayerController PlayerController { get; set; }

    public PlayerPlayerControllerPresender(PlayerModel playerModel, PlayerController playerController)
    {
        PlayerModel = playerModel;
        PlayerController = playerController;
    }
}
