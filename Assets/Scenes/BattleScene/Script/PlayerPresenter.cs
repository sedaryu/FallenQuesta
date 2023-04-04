using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPresenter
{
    private PlayerModel PlayerModel { get; set; } //プレイヤーキャラのステータスを管理するクラス
    private PlayerController PlayerController { get; set; } //プレイヤーキャラのオブジェクトを管理するクラス

    public PlayerPresenter(PlayerModel playerModel, PlayerController playerController)
    {
        PlayerModel = playerModel;
        PlayerController = playerController;
    }

    //残りガッツ量に対応し、Projectileを投げれるかどうか判別
    //投げれる場合、Gutsを消費しtrueを返す(ガッツが足りず投げられない場合はfalseを返す)
    public bool DecreaseGuts(float guts)
    {
        if (PlayerModel.DecreaseGuts(guts)) //渡されたぶんの残りガッツがあればtrue
        {
            PlayerController.UpdateGutsUI(PlayerModel.Guts); //GutsゲージのUIも更新
            return true;
        }
        else
        {
            return false; //渡されたぶんの残りガッツが無ければfalseを返す
        }
    }

    public void RecoverGuts()
    {
        PlayerModel.RecoverGuts(); //時間経過で自動でガッツが回復
        PlayerController.UpdateGutsUI(PlayerModel.Guts); //GutsゲージのUIも更新
    }

    public void DecreaseHp()
    {
        PlayerModel.DecreaseHp(); //Hpが減少
        PlayerController.UpdateHpUI(PlayerModel.Hp); //HpゲージのUIも更新
    }
}
