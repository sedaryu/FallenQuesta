using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed { get; private set; }

    public void Constructor(Player player)
    {
        Speed = player.Speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(0, -3.5f, 0); //�����ʒu��ݒ�
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
        else if (Input.GetKey(KeyCode.Space)) //��������
        {

        }

        if (-9 <= this.transform.position.x + move && this.transform.position.x + move <= 9) //�ړ��������͈͓��Ɏ��܂��Ă��邩�𔻒�
        {
            this.transform.Translate(move, 0, 0); //�v���C���[�̈ړ��𔽉f
        }
    }
}
