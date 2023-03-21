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
        this.transform.position = new Vector3(0, -3.5f, 0); //初期位置を設定
        GutsGauge = GameObject.Find("GutsGauge").GetComponent<GutsGaugeController>();
    }

    // Update is called once per frame
    void Update()
    {
        float move = 0;

        if (Input.GetKey(KeyCode.A)) //Aを押した場合
        {
            move = Speed * Time.deltaTime * -1; //左に移動
        }
        else if (Input.GetKey(KeyCode.D)) //Dを押した場合
        {
            move = Speed * Time.deltaTime * 1; //右に移動
        }

        if (Input.GetKey(KeyCode.Space)) //たたかう
        {

        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            UpArrowKey.Invoke(this, KeyCode.UpArrow);
        }

        if (-9 <= this.transform.position.x + move && this.transform.position.x + move <= 9) //移動が制限範囲内に収まっているかを判定
        {
            this.transform.Translate(move, 0, 0); //プレイヤーの移動を反映
        }
    }

    //プロジェクティルを生成
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
