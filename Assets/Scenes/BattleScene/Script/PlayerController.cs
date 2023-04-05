using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//PlayerController�N���X�ɂ��Ẳ��
//�V�[����ł̃v���C���[�I�u�W�F�N�g�̓���A�܂�UI���Ǘ�����N���X
//�v���C���[�I�u�W�F�N�g��PlayerPrefab����C���X�^���X�����Ɠ����ɁAConstructor���\�b�h�����s����A
//�����Ƃ��Ď󂯎����Player�N���X�ɑ������Ă���e�X�e�[�^�X���v���p�e�B�ɑ������
//�����ɁA�I�u�W�F�N�g�ɃA�^�b�`����Ă���Spriterenderer��sprite�ɁA�e�L�����ŗL��sprite(�L�����摜)��������
//Start���\�b�h�ŁA�V�[����ɔz�u����Ă���eUI(HpGauge�EGutsGauge)���擾����
//UI��PlayerModel�̊e�v���p�e�B(Hp�EGuts)�̕ω��ɘA�����A�l��ω�������

public class PlayerController : MonoBehaviour
{
    public float Speed { get; private set; } //�ړ����x
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
        Speed = player.Speed;
        Projectiles = player.Projectiles;
        //Resources�ɕۑ����ꂽ�L�����N�^�[�摜�����[�h���A�擾����
        playerImage = Resources.Load($"Player/{player.Name}", typeof(Sprite)) as Sprite;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = playerImage; //Resources����擾�����L�����̉摜��SpriteRenderer�ɑ��
    }

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(0, -3.5f, 0); //�����ʒu��ݒ�
        HpGauge = GameObject.Find("HpGauge").GetComponent<HpGaugeController>(); //Hp�Q�[�W��UI���擾
        GutsGauge = GameObject.Find("GutsGauge").GetComponent<GutsGaugeController>(); //Guts�Q�[�W��UI���擾
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
