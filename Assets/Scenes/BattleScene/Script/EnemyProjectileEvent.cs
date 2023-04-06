using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileEvent
{
    Dictionary<string, GameObject> Projectiles; //ProjectilePrefab���܂Ƃ߂�Dictionary
    private PlayerPresenter PlayerPresender { get; set; } //�v���C���[���_���[�W���󂯂��ۂɎg�p
    private Transform PlayerTransform { get; set; } //�����蔻��Ɏg�p

    public EnemyProjectileEvent(PlayerPresenter playerPresender, Transform playerTransform, Dictionary<string, GameObject> projectiles)
    {
        PlayerPresender = playerPresender;
        PlayerTransform = playerTransform;
        Projectiles = projectiles;
    }

    //key��p����Projectiles����w�肵��ProjectilePrefab���擾���A
    //�������ꂽ��ѓ���I�u�W�F�N�g�ɃA�^�b�`���ꂽ�X�N���v�g�ɂ��̏���n��
    //�v���C���[�I�u�W�F�N�g�Ƀq�b�g�����ۂɔ��������郁�\�b�h���C�x���g�֓n��
    public void ThrowProjectile(EnemyController enemyController, string key)
    {
        ProjectileController projectile = enemyController.InstanciateProjectile(Projectiles[key]).GetComponent<ProjectileController>();
        projectile.Constructor(PlayerTransform);
        projectile.ProjectileHitPlayer += DecreasePlayerHp;
    }

    public void DecreasePlayerHp() //�����蔻��̌��ʁA�v���C���[�Ƀ_���[�W��^����ꍇ
    {
        PlayerPresender.DecreaseHp();
    }
}
