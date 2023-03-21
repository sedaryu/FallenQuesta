using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileEvent
{
    private PlayerPresender PlayerPresender { get; set; }
    private List<EnemyPresender> EnemyPresender { get; set; }
    private Dictionary<KeyCode, Projectile> PlayerProjectile { get; set; }
    private List<Transform> EnemyTransforms { get; set; }

    public PlayerProjectileEvent(PlayerPresender playerPresender, List<EnemyPresender> enemyPresender, 
                                 Dictionary<KeyCode, Projectile> playerProjectile, List<Transform> enemyTransforms)
    {
        PlayerPresender = playerPresender;
        EnemyPresender = enemyPresender;
        PlayerProjectile = playerProjectile;
        EnemyTransforms = enemyTransforms;
    }

    public void ThrowProjectile(PlayerController playerController, KeyCode key)
    {
        float cost = PlayerProjectile[key].Cost;

        if (PlayerPresender.DecreaseGuts(cost)) //�v���W�F�N�e�B���̃R�X�g�Ԃ�K�b�c�����邩�m�F
        {
            ProjectileController projectile = playerController.InstanciateProjectile().GetComponent<ProjectileController>();
            projectile.Constructor(EnemyTransforms, PlayerProjectile[key]);
            projectile.ProjectileHitEnemy += DecreaseEnemyDamage;
        }
    }

    public void DecreaseEnemyDamage(List<int> hits, float damage) //�����蔻��̌��ʁA�G�Ƀ_���[�W��^����ꍇ
    {
        hits.ForEach(x => EnemyPresender[x].DecreaseHp(damage));
    }
}
