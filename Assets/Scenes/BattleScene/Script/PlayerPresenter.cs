using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPresenter
{
    private PlayerModel PlayerModel { get; set; }
    private PlayerController PlayerController { get; set; }

    public PlayerPresenter(PlayerModel playerModel, PlayerController playerController)
    {
        PlayerModel = playerModel;
        PlayerController = playerController;
    }

    public bool DecreaseGuts(float guts)
    {
        if (PlayerModel.DecreaseGuts(guts))
        {
            PlayerController.DecreaseGuts(guts);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RecoverGuts()
    { 
        float recover = PlayerController.RecoverGuts();
        PlayerModel.RecoverGuts(recover);
    }

    public void DecreaseHp()
    {
        PlayerModel.DecreaseHp();
        Debug.Log(PlayerModel.Hp);
    }
}
