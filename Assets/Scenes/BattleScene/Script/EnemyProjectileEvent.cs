using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileEvent
{
    Dictionary<string, Projectile> Projectile; //�v���W�F�N�e�B���S�Ă̏����܂Ƃ߂��f�B�N�V���i��
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

    public void DecreasePlayerHp() //�����蔻��̌��ʁA�v���C���[�Ƀ_���[�W��^����ꍇ
    {
        PlayerPresender.DecreaseHp();
    }
}
