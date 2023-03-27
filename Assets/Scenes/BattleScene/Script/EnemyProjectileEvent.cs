using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileEvent
{
    Dictionary<string, Projectile> EnemyProjectile; //場にいるエネミーの放つプロジェクティル全ての情報をまとめたディクショナリ
    private PlayerPresenter PlayerPresender { get; set; }
    private Transform PlayerTransform { get; set; }

    public EnemyProjectileEvent(PlayerPresenter playerPresender, Transform playerTransform, Dictionary<string, Projectile> enemyProjectile)
    {
        PlayerPresender = playerPresender;
        PlayerTransform = playerTransform;
        EnemyProjectile = enemyProjectile;
    }

    public void ThrowProjectile(EnemyController enemyController, string key)
    {
        ProjectileController projectile = enemyController.InstanciateProjectile().GetComponent<ProjectileController>();
        projectile.Constructor(PlayerTransform, EnemyProjectile[key]);
        projectile.ProjectileHitPlayer += DecreasePlayerHp;
    }

    public void DecreasePlayerHp() //当たり判定の結果、プレイヤーにダメージを与える場合
    {
        PlayerPresender.DecreaseHp();
    }
}
