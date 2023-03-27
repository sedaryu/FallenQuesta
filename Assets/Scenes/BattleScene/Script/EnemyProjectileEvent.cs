using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileEvent
{
    Dictionary<string, Projectile> Projectile; //プロジェクティル全ての情報をまとめたディクショナリ
    private PlayerPresenter PlayerPresender { get; set; }
    private Transform PlayerTransform { get; set; }

    public EnemyProjectileEvent(PlayerPresenter playerPresender, Transform playerTransform, Dictionary<string, Projectile> projectile)
    {
        PlayerPresender = playerPresender;
        PlayerTransform = playerTransform;
        Projectile = projectile;
    }

    public void ThrowProjectile(EnemyController enemyController, string key)
    {
        ProjectileController projectile = enemyController.InstanciateProjectile().GetComponent<ProjectileController>();
        projectile.Constructor(PlayerTransform, Projectile[key]);
        projectile.ProjectileHitPlayer += DecreasePlayerHp;
    }

    public void DecreasePlayerHp() //当たり判定の結果、プレイヤーにダメージを与える場合
    {
        PlayerPresender.DecreaseHp();
    }
}
