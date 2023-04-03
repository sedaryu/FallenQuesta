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

    private HpGaugeController HpGauge; //Hpを表示するUI
    private GutsGaugeController GutsGauge; //Gutsを表示するUI
    private SpriteRenderer spriteRenderer; //PrefabにアタッチされているSpriteRendererを格納
    private Sprite playerImage; //立ち絵画像

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
        this.transform.position = new Vector3(0, -3.5f, 0); //初期位置を設定
        HpGauge = GameObject.Find("HpGauge").GetComponent<HpGaugeController>();
        GutsGauge = GameObject.Find("GutsGauge").GetComponent<GutsGaugeController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = playerImage;
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

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            UpArrowKey.Invoke(this, Projectiles[0]);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //LeftArrowKey.Invoke(this, Projectiles[1]);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //DownArrowKey.Invoke(this, Projectiles[2]);

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //RightArrowKey.Invoke(this, Projectiles[3]);

        }

        if (-9 <= this.transform.position.x + move && this.transform.position.x + move <= 9) //移動が制限範囲内に収まっているかを判定
        {
            this.transform.Translate(move, 0, 0); //プレイヤーの移動を反映
        }
    }

    //Projectileを生成
    public GameObject InstanciateProjectile()
    {
        return Instantiate(ProjectilePrefab, this.transform.position, Quaternion.identity);
    }

    public void DecreaseHp(float damage)
    {
        HpGauge.Value -= damage;
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
