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
        if (PlayerModel.DecreaseGuts(guts)) //�n���ꂽ�Ԃ�̎c��K�b�c�������true
        {
            PlayerController.DecreaseGuts(guts); //�c��K�b�c�ɉ�����UI���X�V
            return true;
        }
        else
        {
            return false; //�n���ꂽ�Ԃ�̎c��K�b�c���������false��Ԃ�
        }
    }

    public void RecoverGuts()
    { 
        float recover = PlayerController.RecoverGuts(); //���Ԍo�߂Ŏ����ŃK�b�c����
        PlayerModel.RecoverGuts(recover); //�񕜂����K�b�c�ɉ�����UI���X�V
    }

    public void DecreaseHp()
    {
        float damage = PlayerModel.DecreaseHp(); //Hp������
        PlayerController.DecreaseHp(damage); //�c��Hp�ɉ�����UI���X�V
    }
}
