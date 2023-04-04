using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPresenter
{
    private PlayerModel PlayerModel { get; set; } //�v���C���[�L�����̃X�e�[�^�X���Ǘ�����N���X
    private PlayerController PlayerController { get; set; } //�v���C���[�L�����̃I�u�W�F�N�g���Ǘ�����N���X

    public PlayerPresenter(PlayerModel playerModel, PlayerController playerController)
    {
        PlayerModel = playerModel;
        PlayerController = playerController;
    }

    //�c��K�b�c�ʂɑΉ����AProjectile�𓊂���邩�ǂ�������
    //�������ꍇ�AGuts�����true��Ԃ�(�K�b�c�����肸�������Ȃ��ꍇ��false��Ԃ�)
    public bool DecreaseGuts(float guts)
    {
        if (PlayerModel.DecreaseGuts(guts)) //�n���ꂽ�Ԃ�̎c��K�b�c�������true
        {
            PlayerController.UpdateGutsUI(PlayerModel.Guts); //Guts�Q�[�W��UI���X�V
            return true;
        }
        else
        {
            return false; //�n���ꂽ�Ԃ�̎c��K�b�c���������false��Ԃ�
        }
    }

    public void RecoverGuts()
    {
        PlayerModel.RecoverGuts(); //���Ԍo�߂Ŏ����ŃK�b�c����
        PlayerController.UpdateGutsUI(PlayerModel.Guts); //Guts�Q�[�W��UI���X�V
    }

    public void DecreaseHp()
    {
        PlayerModel.DecreaseHp(); //Hp������
        PlayerController.UpdateHpUI(PlayerModel.Hp); //Hp�Q�[�W��UI���X�V
    }
}
