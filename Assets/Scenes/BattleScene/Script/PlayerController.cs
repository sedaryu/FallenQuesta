using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float Speed { get; private set; }
    public float Recover { get; private set; }

    public event Action<PlayerController, KeyCode> UpArrowKey;
    private GutsGaugeController GutsGauge;

    [SerializeField] private GameObject ProjectilePrefab;

    public void Constructor(Player player)
    {
        Speed = player.Speed;
        Recover = player.Recover;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(0, -3.5f, 0); //�����ʒu��ݒ�
        GutsGauge = GameObject.Find("GutsGauge").GetComponent<GutsGaugeController>();
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
            UpArrowKey.Invoke(this, KeyCode.UpArrow);
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
