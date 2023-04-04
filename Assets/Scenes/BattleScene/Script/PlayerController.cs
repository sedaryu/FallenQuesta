using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�v���C���[�L�����ɂ��Ẳ��

//�v���C���[�L������Hp��0�ȉ��ɂȂ�΃Q�[���I�[�o�[
//Hp�̍ő�l�͊e�L�������ʂ�1
//Defense(��_���[�W��)�̒l�ɂ���Ď󂯂�_���[�W�ʂ��ω�����
//�U�������s������Guts�������
//Guts�͎��Ԍo�߂Ŏ����񕜂���
//Projectile���Ƃɏ����Guts�͈قȂ�

public class PlayerController : MonoBehaviour
{
    public float Defense { get; private set; } //���e������̔�_���[�W��
    public float Speed { get; private set; } //�ړ����x
    public float Recover { get; private set; } //��b�Ԃɉ񕜂���K�b�c��
    public List<string> Projectiles { get; private set; } //�g�p����Projectile��

    public event Action<PlayerController, string> UpArrowKey; //UpArrow�L�[���������ہA���s�����C�x���g
    public event Action<PlayerController, string> LeftArrowKey; //LeftArrow�L�[���������ہA���s�����C�x���g
    public event Action<PlayerController, string> DownArrowKey; //DownArrow�L�[���������ہA���s�����C�x���g
    public event Action<PlayerController, string> RightArrowKey; //RightArrow�L�[���������ہA���s�����C�x���g

    private HpGaugeController HpGauge; //Hp��\������UI
    private GutsGaugeController GutsGauge; //Guts��\������UI
    private SpriteRenderer spriteRenderer; //Prefab�ɃA�^�b�`����Ă���SpriteRenderer���i�[
    private Sprite playerImage; //�����G�摜

    [SerializeField] private GameObject ProjectilePrefab; //Projectile�𐶐�����ہA�C���X�^���X����Prefab

    public void Constructor(Player player)
    {
        Defense = player.Defense;
        Speed = player.Speed;
        Recover = player.Recover;
        Projectiles = player.Projectiles;
        //Resources�ɕۑ����ꂽ�L�����N�^�[�摜�����[�h���A�擾����
        playerImage = Resources.Load($"Player/{player.Name}", typeof(Sprite)) as Sprite;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(0, -3.5f, 0); //�����ʒu��ݒ�
        HpGauge = GameObject.Find("HpGauge").GetComponent<HpGaugeController>(); //Hp�Q�[�W��UI���擾
        GutsGauge = GameObject.Find("GutsGauge").GetComponent<GutsGaugeController>(); //Guts�Q�[�W��UI���擾
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = playerImage; //Resources����擾�����L�����̉摜��SpriteRenderer�ɑ��
    }

    // Update is called once per frame
    void Update()
    {
        Move(); //�ړ������s
        Attack(); //�U�������s
    }

    private void Move() //�I�u�W�F�N�g�̈ړ������s���郁�\�b�h
    {
        float move = 0;

        if (Input.GetKey(KeyCode.A)) //A���������ꍇ
        {
            move = Speed * Time.deltaTime * -1; //���Ɉړ�
        }
        else if (Input.GetKey(KeyCode.D)) //D���������ꍇ
        {
            move = Speed * Time.deltaTime * 1; //�E�Ɉړ�
        }

        if (-9 <= this.transform.position.x + move && this.transform.position.x + move <= 9) //�ړ��������͈͓��Ɏ��܂��Ă��邩�𔻒�
        {
            this.transform.Translate(move, 0, 0); //�v���C���[�̈ړ��𔽉f
        }
    }

    private void Attack() //�I�u�W�F�N�g�̍U�������s���郁�\�b�h
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) //UpArrow���������ꍇ
        {
            UpArrowKey.Invoke(this, Projectiles[0]);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))  //LeftArrow���������ꍇ
        {
            //LeftArrowKey.Invoke(this, );
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))  //DownArrow���������ꍇ
        {
            //DownArrowKey.Invoke(this, );
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) //RightArrow���������ꍇ
        {
            //RightArrowKey.Invoke(this, );
        }
    }

    //ProjectilePrefab����Projectile�I�u�W�F�N�g�𐶐�
    public GameObject InstanciateProjectile() //ProjectileEvent�N���X��ThrowProjectile���\�b�h����Ă΂��
    {
        return Instantiate(ProjectilePrefab, this.transform.position, Quaternion.identity);
    }

    public void UpdateHpUI(float hp) //Hp�Q�[�W��UI���X�V
    {
        HpGauge.Value = hp;
    }

    public void UpdateGutsUI(float guts) //Guts�Q�[�W��UI���X�V
    {
        GutsGauge.Value = guts;
    }
}
