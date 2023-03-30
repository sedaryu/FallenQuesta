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
        EnemyModel.DecreaseHp(damage); //受けたダメージぶんHpが減少

        //if (EnemyModel.Hp <= 0) //Hpが0以下になった場合
        //{
        //    EnemyController.DeadEnemy(); //死亡(非アクティブ化)
        //}

        EnemyController.hpGauge.value -= damage; //残りHpに応じてUIが更新
    }

    public void SelfHealing(float deltatime)
    { 
        float heal = EnemyModel.SelfHealing(deltatime); //時間経過で自動でHpが回復
        EnemyController.hpGauge.value += heal; //回復したHpに応じてUIが更新
    }
}
