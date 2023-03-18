using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPresender
{
    private EnemyModel EnemyModel { get; set; }
    private EnemyController EnemyController { get; set; }

    public EnemyPresender(EnemyModel enemyModel, EnemyController enemyController)
    {
        EnemyModel = enemyModel;
        EnemyController = enemyController;
    }
}
