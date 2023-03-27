using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileEvent
{
    private PlayerPresenter PlayerPresender { get; set; }
    private List<EnemyPresenter> EnemyPresender { get; set; }
    private Dictionary<string, Projectile> Projectile { get; set; }
    private List<Transform> EnemyTransforms { get; set; }

    public PlayerProjectileEvent(PlayerPresenter playerPresender, List<EnemyPresenter> enemyPresender, 
                                 Dictionary<string, Projectile> projectile, List<Transform> enemyTransforms)
    {
        PlayerPresender = playerPresender;
        EnemyPresender = enemyPresender;
        Projectile = projectile;
        EnemyTransforms = enemyTransforms;
    }

    public void ThrowProjectile(PlayerController playerController, string key)
    {
        float cost = Projectile[key].Cost;

        if (PlayerPresender.DecreaseGuts(cost)) //プロジェクティルのコストぶんガッツがあるか確認
        {
            ProjectileController projectile = playerController.InstanciateProjectile().GetComponent<ProjectileController>();
            projectile.Constructor(EnemyTransforms, Projectile[key]);
            projectile.ProjectileHitEnemy += DecreaseEnemyDamage;
        }
    }

    public void DecreaseEnemyDamage(List<int> hits, float damage) //当たり判定の結果、敵にダメージを与える場合
    {
        hits.ForEach(x => EnemyPresender[x].DecreaseHp(damage));
    }
}
