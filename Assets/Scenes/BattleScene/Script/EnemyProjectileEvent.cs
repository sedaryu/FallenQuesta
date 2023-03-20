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

        if (PlayerPresender.DecreaseGuts(cost)) //�v���W�F�N�e�B���̃R�X�g�Ԃ�K�b�c�����邩�m�F
        {
            ProjectileController projectile = playerController.InstanciateProjectile().GetComponent<ProjectileController>();
            projectile.Constructor(EnemyTransforms, PlayerProjectile[key]);
            projectile.ProjectileHit += DecreaseEnemyDamage;
        }
    }

    public void DecreaseEnemyDamage(List<int> hits, float damage) //�����蔻��̌��ʁA�G�Ƀ_���[�W��^����ꍇ
    {
        hits.ForEach(x => EnemyPresender[x].DecreaseHp(damage));
    }
}
