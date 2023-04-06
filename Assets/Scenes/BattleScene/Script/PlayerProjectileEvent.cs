using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerProjectileEvent
{
    private PlayerPresenter PlayerPresenter { get; set; }
    private List<EnemyPresenter> EnemyPresenter { get; set; }
    private Dictionary<string, GameObject> Projectiles { get; set; } //ProjectilePrefab���܂Ƃ߂�Dictionary
    private Dictionary<string, float> ProjectileCosts { get; set; } = new Dictionary<string, float>();
    private List<Transform> EnemyTransforms { get; set; } //�����蔻��Ɏg�p

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

    //key��p����Projectiles����w�肵��ProjectilePrefab���擾���A
    //�������ꂽ��ѓ���I�u�W�F�N�g�ɃA�^�b�`���ꂽ�X�N���v�g�ɂ��̏���n��
    //�G�l�~�[�I�u�W�F�N�g�Ƀq�b�g�����ۂɔ��������郁�\�b�h���C�x���g�֓n��
    public void ThrowProjectile(PlayerController playerController, string key)
    {
        float cost = ProjectileCosts[key];

        if (PlayerPresenter.DecreaseGuts(cost)) //Projectile�̃R�X�g�Ԃ�K�b�c�����邩�m�F
        {
            ProjectileController projectile = playerController.InstanciateProjectile(Projectiles[key]).GetComponent<ProjectileController>();
            projectile.Constructor(EnemyTransforms);
            projectile.ProjectileHitEnemy += DecreaseEnemyDamage;
        }
    }

    public void DecreaseEnemyDamage(List<int> hits, float damage) //�����蔻��̌��ʁA�G�Ƀ_���[�W��^����ꍇ
    {
        hits.ForEach(x => EnemyPresenter[x].DecreaseHp(damage));
    }
}
