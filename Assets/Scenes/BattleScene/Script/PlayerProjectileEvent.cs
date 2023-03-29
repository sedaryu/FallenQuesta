using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileEvent
{
    private PlayerPresenter PlayerPresender { get; set; }
    private List<EnemyPresenter> EnemyPresender { get; set; }
    private Dictionary<string, Projectile> Projectile { get; set; } //Projectile�S�Ă̏����܂Ƃ߂�Dictionary
    private List<Transform> EnemyTransforms { get; set; } //�����蔻��Ɏg�p

    public PlayerProjectileEvent(PlayerPresenter playerPresender, List<EnemyPresenter> enemyPresender, 
                                 Dictionary<string, Projectile> projectile, List<Transform> enemyTransforms)
    {
        PlayerPresender = playerPresender;
        EnemyPresender = enemyPresender;
        Projectile = projectile;
        EnemyTransforms = enemyTransforms;
    }

    //key��p����ProjectileDictionary����w�肵��Projectile�̃X�e�[�^�X���擾���A�������ꂽProjectilePrefab�ɓn��
    public void ThrowProjectile(PlayerController playerController, string key)
    {
        float cost = Projectile[key].Cost;

        if (PlayerPresender.DecreaseGuts(cost)) //Projectile�̃R�X�g�Ԃ�K�b�c�����邩�m�F
        {
            ProjectileController projectile = playerController.InstanciateProjectile().GetComponent<ProjectileController>();
            projectile.Constructor(EnemyTransforms, Projectile[key]);
            projectile.ProjectileHitEnemy += DecreaseEnemyDamage;
        }
    }

    public void DecreaseEnemyDamage(List<int> hits, float damage) //�����蔻��̌��ʁA�G�Ƀ_���[�W��^����ꍇ
    {
        hits.ForEach(x => EnemyPresender[x].DecreaseHp(damage));
    }
}
