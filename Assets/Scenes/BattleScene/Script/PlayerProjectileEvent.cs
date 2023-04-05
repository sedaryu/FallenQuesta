using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileEvent
{
    private PlayerPresenter PlayerPresenter { get; set; }
    private List<EnemyPresenter> EnemyPresenter { get; set; }
    private Dictionary<string, Projectile> Projectile { get; set; } //Projectile�̏����܂Ƃ߂�Dictionary
    private List<Transform> EnemyTransforms { get; set; } //�����蔻��Ɏg�p

    public PlayerProjectileEvent(PlayerPresenter playerPresender, List<EnemyPresenter> enemyPresender, 
                                 Dictionary<string, Projectile> projectile, List<Transform> enemyTransforms)
    {
        PlayerPresenter = playerPresender;
        EnemyPresenter = enemyPresender;
        Projectile = projectile;
        EnemyTransforms = enemyTransforms;
    }

    //key��p����ProjectileDictionary����w�肵��Projectile�̃X�e�[�^�X���擾���A
    //�������ꂽ��ѓ���I�u�W�F�N�g�ɃA�^�b�`���ꂽ�X�N���v�g�ɂ��̏���n��
    //�G�l�~�[�I�u�W�F�N�g�Ƀq�b�g�����ۂɔ��������郁�\�b�h���C�x���g�֓n��
    public void ThrowProjectile(PlayerController playerController, string key)
    {
        float cost = Projectile[key].Cost;

        if (PlayerPresenter.DecreaseGuts(cost)) //Projectile�̃R�X�g�Ԃ�K�b�c�����邩�m�F
        {
            ProjectileController projectile = playerController.InstanciateProjectile().GetComponent<ProjectileController>();
            projectile.Constructor(EnemyTransforms, Projectile[key]);
            projectile.ProjectileHitEnemy += DecreaseEnemyDamage;
        }
    }

    public void DecreaseEnemyDamage(List<int> hits, float damage) //�����蔻��̌��ʁA�G�Ƀ_���[�W��^����ꍇ
    {
        hits.ForEach(x => EnemyPresenter[x].DecreaseHp(damage));
    }
}
