using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerProjectileEvent
{
    private PlayerPresenter PlayerPresenter { get; set; }
    private List<EnemyPresenter> EnemyPresenter { get; set; }
    private Dictionary<string, GameObject> Projectiles { get; set; } //ProjectilePrefabをまとめたDictionary
    private Dictionary<string, float> ProjectileCosts { get; set; } = new Dictionary<string, float>();
    private List<Transform> EnemyTransforms { get; set; } //当たり判定に使用

    public PlayerProjectileEvent(PlayerPresenter playerPresender, List<EnemyPresenter> enemyPresender, 
                                 Dictionary<string, GameObject> projectiles, List<Transform> enemyTransforms)
    {
        PlayerPresenter = playerPresender;
        EnemyPresenter = enemyPresender;
        Projectiles = projectiles;
        Projectiles.Values.ToList()
                          .ForEach(x => ProjectileCosts.Add(x.GetComponent<ProjectileController>().Name, x.GetComponent<ProjectileController>().Cost));
        EnemyTransforms = enemyTransforms;
    }

    //keyを用いてProjectilesから指定したProjectilePrefabを取得し、
    //生成された飛び道具オブジェクトにアタッチされたスクリプトにその情報を渡す
    //エネミーオブジェクトにヒットした際に発生させるメソッドもイベントへ渡す
    public void ThrowProjectile(PlayerController playerController, string key)
    {
        float cost = ProjectileCosts[key];

        if (PlayerPresenter.DecreaseGuts(cost)) //Projectileのコストぶんガッツがあるか確認
        {
            ProjectileController projectile = playerController.InstanciateProjectile(Projectiles[key]).GetComponent<ProjectileController>();
            projectile.Constructor(EnemyTransforms);
            projectile.ProjectileHitEnemy += DecreaseEnemyDamage;
        }
    }

    public void DecreaseEnemyDamage(List<int> hits, float damage) //当たり判定の結果、敵にダメージを与える場合
    {
        hits.ForEach(x => EnemyPresenter[x].DecreaseHp(damage));
    }
}
