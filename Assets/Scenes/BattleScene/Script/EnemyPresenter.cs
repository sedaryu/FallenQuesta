using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPresenter
{
    private EnemyModel EnemyModel { get; set; } //�G�l�~�[�L�����̃X�e�[�^�X���Ǘ�����N���X
    private EnemyController EnemyController { get; set; } //�G�l�~�[�L�����̃I�u�W�F�N�g���Ǘ�����N���X

    public EnemyPresenter(EnemyModel enemyModel, EnemyController enemyController)
    {       
        EnemyModel = enemyModel;
        EnemyController = enemyController;
    }

    public void DecreaseHp(float damage)
    {
        EnemyModel.DecreaseHp(damage); //�󂯂��_���[�W�Ԃ�Hp������
        EnemyController.UpdateHpUI(EnemyModel.Hp); //�c��Hp�ɉ�����UI���X�V
    }

    public void SelfHealing()
    { 
        EnemyModel.SelfHealing(); //���Ԍo�߂Ŏ�����Hp����
        EnemyController.UpdateHpUI(EnemyModel.Hp); //�񕜂���Hp�ɉ�����UI���X�V
    }
}
