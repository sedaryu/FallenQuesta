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
        if (PlayerModel.DecreaseGuts(guts)) //渡されたぶんの残りガッツがあればtrue
        {
            PlayerController.DecreaseGuts(guts); //残りガッツに応じてUIを更新
            return true;
        }
        else
        {
            return false; //渡されたぶんの残りガッツが無ければfalseを返す
        }
    }

    public void RecoverGuts()
    { 
        float recover = PlayerController.RecoverGuts(); //時間経過で自動でガッツが回復
        PlayerModel.RecoverGuts(recover); //回復したガッツに応じてUIが更新
    }

    public void DecreaseHp()
    {
        float damage = PlayerModel.DecreaseHp(); //Hpが減少
        PlayerController.DecreaseHp(damage); //残りHpに応じてUIが更新
    }
}
