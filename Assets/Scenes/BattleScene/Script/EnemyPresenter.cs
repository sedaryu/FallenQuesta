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
        EnemyModel.DecreaseHp(damage);

        if (EnemyModel.Hp <= 0)
        {
            EnemyController.DeadEnemy();
        }

        EnemyController.hpGauge.value -= damage;
    }

    public void SelfHealing(float deltatime)
    { 
        float heal = EnemyModel.SelfHealing(deltatime);
        EnemyController.hpGauge.value += heal;
    }
}
