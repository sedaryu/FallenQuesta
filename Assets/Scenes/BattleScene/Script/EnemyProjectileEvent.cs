using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileEvent
{
    Dictionary<string, Projectile> EnemyProjectile; 
    private PlayerPresender PlayerPresender { get; set; }
    private Transform PlayerTransform { get; set; }

    public EnemyProjectileEvent(PlayerPresender playerPresender, Transform playerTransform)
    {
        PlayerPresender = playerPresender;
        PlayerTransform = playerTransform;
    }

    public void ThrowProjectile(EnemyController enemyController, string key)
    {
        float cost = PlayerProjectile[key].Cost;

        if (PlayerPresender.DecreaseGuts(cost)) //プロジェクティルのコストぶんガッツがあるか確認
        {
            ProjectileController projectile = playerController.InstanciateProjectile().GetComponent<ProjectileController>();
            projectile.Constructor(EnemyTransforms, PlayerProjectile[key]);
            projectile.ProjectileHit += DecreaseEnemyDamage;
        }
    }

    public void DecreaseEnemyDamage(List<int> hits, float damage) //当たり判定の結果、敵にダメージを与える場合
    {
        hits.ForEach(x => EnemyPresender[x].DecreaseHp(damage));
    }
}
