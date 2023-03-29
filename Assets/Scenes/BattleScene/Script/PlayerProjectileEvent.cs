using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileEvent
{
    private PlayerPresenter PlayerPresender { get; set; }
    private List<EnemyPresenter> EnemyPresender { get; set; }
    private Dictionary<string, Projectile> Projectile { get; set; } //Projectile全ての情報をまとめたDictionary
    private List<Transform> EnemyTransforms { get; set; } //当たり判定に使用

    public PlayerProjectileEvent(PlayerPresenter playerPresender, List<EnemyPresenter> enemyPresender, 
                                 Dictionary<string, Projectile> projectile, List<Transform> enemyTransforms)
    {
        PlayerPresender = playerPresender;
        EnemyPresender = enemyPresender;
        Projectile = projectile;
        EnemyTransforms = enemyTransforms;
    }

    //keyを用いてProjectileDictionaryから指定したProjectileのステータスを取得し、生成されたProjectilePrefabに渡す
    public void ThrowProjectile(PlayerController playerController, string key)
    {
        float cost = Projectile[key].Cost;

        if (PlayerPresender.DecreaseGuts(cost)) //Projectileのコストぶんガッツがあるか確認
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
