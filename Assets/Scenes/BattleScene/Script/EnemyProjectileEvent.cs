using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileEvent
{
    Dictionary<string, Projectile> Projectile; //Projectile全ての情報をまとめたDictionary
    private PlayerPresenter PlayerPresender { get; set; } //プレイヤーがダメージを受けた際に使用
    private Transform PlayerTransform { get; set; } //当たり判定に使用

    public EnemyProjectileEvent(PlayerPresenter playerPresender, Transform playerTransform, Dictionary<string, Projectile> projectile)
    {
        PlayerPresender = playerPresender;
        PlayerTransform = playerTransform;
        Projectile = projectile;
    }

    //keyを用いてProjectileDictionaryから指定したProjectileのステータスを取得し、生成されたProjectilePrefabに渡す
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
