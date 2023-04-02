using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPresenter
{
    private EnemyModel EnemyModel { get; set; }
    private EnemyController EnemyController { get; set; }

    public EnemyPresenter(EnemyModel enemyModel, EnemyController enemyController)
    {
        EnemyModel = enemyModel;
        EnemyController = enemyController;
    }

    public void DecreaseHp(float damage)
    {
        EnemyModel.DecreaseHp(damage); //�󂯂��_���[�W�Ԃ�Hp������

        EnemyController.hpGauge.value -= damage; //�c��Hp�ɉ�����UI���X�V
    }

    public void SelfHealing(float deltatime)
    { 
        float heal = EnemyModel.SelfHealing(deltatime); //���Ԍo�߂Ŏ�����Hp����
        EnemyController.hpGauge.value += heal; //�񕜂���Hp�ɉ�����UI���X�V
    }
}
