using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileEvent
{
    Dictionary<string, GameObject> Projectiles; //ProjectilePrefabをまとめたDictionary
    private PlayerPresenter PlayerPresender { get; set; } //プレイヤーがダメージを受けた際に使用
    private Transform PlayerTransform { get; set; } //当たり判定に使用

    public EnemyProjectileEvent(PlayerPresenter playerPresender, Transform playerTransform, Dictionary<string, GameObject> projectiles)
    {
        PlayerPresender = playerPresender;
        PlayerTransform = playerTransform;
        Projectiles = projectiles;
    }

    //keyを用いてProjectileDictionaryから指定したProjectileのステータスを取得し、生成されたProjectilePrefabに渡す
    public void ThrowProjectile(EnemyController enemyController, string key)
    {
        ProjectileController projectile = enemyController.InstanciateProjectile(Projectiles[key]).GetComponent<ProjectileController>();
        projectile.Constructor(PlayerTransform);
        projectile.ProjectileHitPlayer += DecreasePlayerHp;
    }

    public void DecreasePlayerHp() //当たり判定の結果、プレイヤーにダメージを与える場合
    {
        PlayerPresender.DecreaseHp();
    }
}
