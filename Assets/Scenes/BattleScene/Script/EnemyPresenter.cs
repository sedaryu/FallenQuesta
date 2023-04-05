using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPresenter
{
    private EnemyModel EnemyModel { get; set; } //エネミーキャラのステータスを管理するクラス
    private EnemyController EnemyController { get; set; } //エネミーキャラのオブジェクトを管理するクラス

    public EnemyPresenter(EnemyModel enemyModel, EnemyController enemyController)
    {       
        EnemyModel = enemyModel;
        EnemyController = enemyController;
    }

    public void DecreaseHp(float damage)
    {
        EnemyModel.DecreaseHp(damage); //受けたダメージぶんHpが減少
        EnemyController.UpdateHpUI(EnemyModel.Hp); //残りHpに応じてUIが更新
    }

    public void SelfHealing()
    { 
        EnemyModel.SelfHealing(); //時間経過で自動でHpが回復
        EnemyController.UpdateHpUI(EnemyModel.Hp); //回復したHpに応じてUIが更新
    }
}
