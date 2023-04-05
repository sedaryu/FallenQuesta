using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//ProjectileController�N���X�ɂ��Ẳ��
//�V�[����ł̔�ѓ���I�u�W�F�N�g�̓�����Ǘ�����N���X
//��ѓ���I�u�W�F�N�g��ProjectilePrefab����C���X�^���X�����Ɠ����ɁAConstructor���\�b�h�����s����A
//�����Ƃ��Ď󂯎����Projectile�N���X�ɑ������Ă���e�X�e�[�^�X���AProjectileCalculator�N���X�̃v���p�e�B�ɑ������
//ProjectileCalculator�N���X

public class ProjectileController : MonoBehaviour
{
    private bool Player { get; set; } //�v���C���[��������Projectile�̏ꍇtrue�A�G�l�~�[�̏ꍇ��false
    private Transform TargetTransform { get; set; } //Projectile�̃^�[�Q�b�g�ƂȂ�I�u�W�F�N�g�̈ʒu�A�����蔻��ɗp����
    private List<Transform> TargetTransforms { get; set; } //�^�[�Q�b�g�ƂȂ�I�u�W�F�N�g�������̏ꍇ�͂�����ɑ��
    private ProjectileCalculator ProjectileCalc { get; set; } //Projectile�̈ړ��A�����蔻��Ȃǂ̏������s��

    public event Action ProjectileHitPlayer; //�G�l�~�[��Projectile���v���C���[�ɓ��������ꍇ����
    public event Action<List<int>, float> ProjectileHitEnemy; //�v���C���[��Projectile���G�l�~�[�ɓ��������ꍇ����
                                                              //����List<int>�͓��������G�l�~�[�̃C���f�b�N�X�ԍ�
                                                              //����float�̓G�l�~�[�ɗ^����_���[�W
    public void Constructor(List<Transform> targetTransforms, Projectile projectile)
    {
        TargetTransforms = targetTransforms;
        ProjectileCalc = new ProjectileCalculator(projectile);
        Player = projectile.Player;
    }
    public void Constructor(Transform targetTransform, Projectile projectile)
    {
        TargetTransform = targetTransform;
        ProjectileCalc = new ProjectileCalculator(projectile);
        Player = projectile.Player;
    }

    public void SetTargetTransforms(List<Transform> targetTransforms)
    {
        TargetTransforms = targetTransforms;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Resources����擾������ѓ���̉摜��SpriteRenderer�ɑ��
        this.GetComponent<SpriteRenderer>().sprite = Resources.Load($"Projectile/{ProjectileCalc.Name}", typeof(Sprite)) as Sprite;
        SettingTransform(); //�v���C���[���G�l�~�[���A�ǂ��炪������Projectile���Ńg�����X�t�H�[���𒲐�
    }

    // Update is called once per frame
    void Update()
    {
        ProjectileFlying(); //Projectile�𓮂���

        ProjectileHittingTarget(); //�����蔻��

        JudgeDestroy(); //���͈͂𒴂�����j�󂷂�
    }

    private void SettingTransform()
    {
        if (Player)
        {
            this.transform.position = new Vector3(this.transform.position.x, -3.5f, 0);
            this.transform.localScale = new Vector3(1, 1, 1);
            this.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            this.transform.position = new Vector3(this.transform.position.x, -1, 0);
            this.transform.localScale = new Vector3(0.2f, 0.2f, 1);
            this.transform.rotation = new Quaternion(180, 0, 0, 0);
        }
    }

    private void ProjectileFlying()
    {
        float[] move = new float[3];
        move = ProjectileCalc.Fly(); //ProjectileCalc�ňړ��v�Z���s��
        this.transform.Translate(0, move[0] * Time.deltaTime, 0); //�ʒu���X�V
        this.transform.Rotate(move[1] * Time.deltaTime, 0, 0); //�p�x���X�V
        this.transform.localScale += new Vector3(move[2] * Time.deltaTime, move[2] * Time.deltaTime, 0); //�傫�����X�V

        ProjectileCalc.UpdateTime(Time.deltaTime); //ProjectileCalc�Ɉړ����Ԃ�n��
    }

    private void ProjectileHittingTarget()
    {
        if (Player)
        {
            //�^�[�Q�b�g�̃g�����X�t�H�[������position.x�AlossyScale.x�����𔲂��o��
            List<float> targets_x = TargetTransforms.Select(x => x.position.x).ToList();
            List<float> targets_scale = TargetTransforms.Select(x => x.lossyScale.x).ToList();
            //�����Ƃ��Ċ֐��֓n��
            List<int> hits = ProjectileCalc.JudgeHit(this.transform.position.x, targets_x, targets_scale);

            if (hits.Count > 0) //hits�̗v�f����1�ȏ�ł���΃q�b�g
            {
                ProjectileHitEnemy.Invoke(hits, ProjectileCalc.Attack); //�C�x���g�����s
                Destroy(gameObject);
            }
        }
        else
        {
            bool hit = ProjectileCalc.JudgeHit(this.transform.position.x, this.transform.position.y, 
                                                this.transform.lossyScale.x * 0.5f, this.transform.lossyScale.y * 0.5f, 
                                                TargetTransform.position.x, TargetTransform.lossyScale.x * 0.5f);
            if (hit) //�G�l�~�[�̃v���W�F�N�e�B�����v���C���[�ɓ��������ꍇ�Ahit��true
            {
                ProjectileHitPlayer.Invoke(); //�C�x���g�����s
                Destroy(gameObject);
            }
        }
    }

    private void JudgeDestroy() //���͈͂𒴂����ꍇ�j�󂷂�
    {
        if (Player)
        {
            if (this.transform.position.y > 0)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            if (this.transform.position.y < -7)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
