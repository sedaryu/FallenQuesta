using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//ProjectileController�N���X�ɂ��Ẳ��
//�V�[����ł̔�ѓ���I�u�W�F�N�g�̓�����Ǘ�����N���X
//��ѓ���I�u�W�F�N�g��ProjectilePrefab����C���X�^���X�����Ɠ����ɁAConstructor���\�b�h�����s����A
//�����Ƃ��Ď󂯎����Projectile�N���X�ɑ������Ă���e�X�e�[�^�X���AProjectileCalculator�N���X�̃v���p�e�B�ɑ������
//�����Ƀ^�[�Q�b�g�ƂȂ�L�����N�^�[��Transform���擾����
//

public class ProjectileController : MonoBehaviour
{
    //�C���X�y�N�^�[����������
    [SerializeField] private string _name;
    [SerializeField] private bool _player;
    [SerializeField] private float _attack;
    [SerializeField] private float _cost;
    [SerializeField] private float _flyingTime;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotation;
    [SerializeField] private float _scale;

    public string Name { get => _name; private set => _name = value; }
    public bool Player { get => _player; private set => _player = value; } //�v���C���[��������Projectile�̏ꍇtrue�A�G�l�~�[�̏ꍇ��false
    public float Attack { get => _attack; private set => _attack = value; }
    public float Cost { get => _cost; private set => _cost = value; }
    public float FlyingTime { get => _flyingTime; private set => _flyingTime = value; }
    public float Speed { get => _speed; private set => _speed = value; }
    public float Rotation { get => _rotation; private set => _rotation = value; }
    public float Scale { get => _scale; private set => _scale = value; }

    private Transform TargetTransform { get; set; } //Projectile�̃^�[�Q�b�g�ƂȂ�I�u�W�F�N�g�̈ʒu�A�����蔻��ɗp����
    private List<Transform> TargetTransforms { get; set; } //�^�[�Q�b�g�ƂȂ�I�u�W�F�N�g�������̏ꍇ�͂�����ɑ��
    private ProjectileCalculator ProjectileCalc { get; set; } //Projectile�̈ړ��A�����蔻��Ȃǂ̌v�Z�������s��

    public event Action ProjectileHitPlayer; //�G�l�~�[��Projectile���v���C���[�ɓ��������ꍇ����

    //�v���C���[��Projectile���G�l�~�[�ɓ��������ꍇ����
    //����List<int>�͓��������G�l�~�[�̃C���f�b�N�X�ԍ�
    //����float�̓G�l�~�[�ɗ^����_���[�W
    public event Action<List<int>, float> ProjectileHitEnemy;

    //�^�[�Q�b�g��Transform���擾
    public void Constructor(List<Transform> targetTransforms) //�^�[�Q�b�g�������̏ꍇ
    {
        TargetTransforms = targetTransforms;
    }
    public void Constructor(Transform targetTransform) //�^�[�Q�b�g���P�Ƃ̏ꍇ
    {
        TargetTransform = targetTransform;
    }

    public void SetTargetTransforms(List<Transform> targetTransforms) //�^�[�Q�b�g��ύX�������ꍇ
    {
        TargetTransforms = targetTransforms;
    }

    // Start is called before the first frame update
    void Start()
    {
        ProjectileCalc = new ProjectileCalculator(Player, FlyingTime, Speed, Rotation, Scale);
        SettingTransform(); //�v���C���[���G�l�~�[���A�ǂ��炪������Projectile����Transform�𒲐�
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
                ProjectileHitEnemy.Invoke(hits, Attack); //�C�x���g�����s
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
