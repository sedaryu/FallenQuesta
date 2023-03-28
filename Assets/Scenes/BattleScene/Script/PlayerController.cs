using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float Speed { get; private set; }
    public float Recover { get; private set; }
    public List<string> Projectiles { get; private set; }

    public event Action<PlayerController, string> UpArrowKey;
    public event Action<PlayerController, string> LeftArrowKey;
    public event Action<PlayerController, string> DownArrowKey;
    public event Action<PlayerController, string> RightArrowKey;

    private GutsGaugeController GutsGauge; //UI
    private SpriteRenderer spriteRenderer; //Prefab�ɃA�^�b�`����Ă���X�v���C�g�����_���[���i�[
    private Sprite playerImage; //�����G�摜

    [SerializeField] private GameObject ProjectilePrefab;

    public void Constructor(Player player)
    {
        Speed = player.Speed;
        Recover = player.Recover;
        Projectiles = player.Projectiles;
        playerImage = Resources.Load($"Player/{player.Name}", typeof(Sprite)) as Sprite;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(0, -3.5f, 0); //�����ʒu��ݒ�
        GutsGauge = GameObject.Find("GutsGauge").GetComponent<GutsGaugeController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = playerImage;
    }

    // Update is called once per frame
    void Update()
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

        if (Input.GetKey(KeyCode.Space)) //��������
        {

        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            UpArrowKey.Invoke(this, Projectiles[0]);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (Projectiles.Count > 1)
            {
                UpArrowKey.Invoke(this, Projectiles[1]);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Projectiles.Count > 2)
            { 
                UpArrowKey.Invoke(this, Projectiles[2]);
            }
                
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Projectiles.Count > 3)
            { 
                UpArrowKey.Invoke(this, Projectiles[3]);
            }
            
        }

        if (-9 <= this.transform.position.x + move && this.transform.position.x + move <= 9) //�ړ��������͈͓��Ɏ��܂��Ă��邩�𔻒�
        {
            this.transform.Translate(move, 0, 0); //�v���C���[�̈ړ��𔽉f
        }
    }

    //�v���W�F�N�e�B���𐶐�
    public GameObject InstanciateProjectile()
    {
        return Instantiate(ProjectilePrefab, this.transform.position, Quaternion.identity);
    }

    public void DecreaseGuts(float guts)
    {
        GutsGauge.Value -= guts;
    }

    public float RecoverGuts()
    {
        float recover = Recover * Time.deltaTime;
        GutsGauge.Value += recover;
        return recover;
    }
}
