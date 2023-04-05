using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//PlayerControllerクラスについての解説
//シーン上でのプレイヤーオブジェクトの動作、またUIを管理するクラス
//プレイヤーオブジェクトがPlayerPrefabからインスタンスされると同時に、Constructorメソッドが実行され、
//引数として受け取ったPlayerクラスに代入されている各ステータスをプロパティに代入する
//同時に、オブジェクトにアタッチされているSpriterendererのspriteに、各キャラ固有のsprite(キャラ画像)を代入する
//Startメソッドで、シーン上に配置されている各UI(HpGauge・GutsGauge)を取得する
//UIはPlayerModelの各プロパティ(Hp・Guts)の変化に連動し、値を変化させる

public class PlayerController : MonoBehaviour
{
    public float Speed { get; private set; } //移動速度
    public List<string> Projectiles { get; private set; } //使用するProjectile名

    public event Action<PlayerController, string> UpArrowKey; //UpArrowキーを押した際、実行されるイベント
    public event Action<PlayerController, string> LeftArrowKey; //LeftArrowキーを押した際、実行されるイベント
    public event Action<PlayerController, string> DownArrowKey; //DownArrowキーを押した際、実行されるイベント
    public event Action<PlayerController, string> RightArrowKey; //RightArrowキーを押した際、実行されるイベント

    private HpGaugeController HpGauge; //Hpを表示するUI
    private GutsGaugeController GutsGauge; //Gutsを表示するUI
    private SpriteRenderer spriteRenderer; //PrefabにアタッチされているSpriteRendererを格納
    private Sprite playerImage; //立ち絵画像

    [SerializeField] private GameObject ProjectilePrefab; //Projectileを生成する際、インスタンスするPrefab

    public void Constructor(Player player)
    {
        Speed = player.Speed;
        Projectiles = player.Projectiles;
        //Resourcesに保存されたキャラクター画像をロードし、取得する
        playerImage = Resources.Load($"Player/{player.Name}", typeof(Sprite)) as Sprite;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = playerImage; //Resourcesから取得したキャラの画像をSpriteRendererに代入
    }

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(0, -3.5f, 0); //初期位置を設定
        HpGauge = GameObject.Find("HpGauge").GetComponent<HpGaugeController>(); //HpゲージのUIを取得
        GutsGauge = GameObject.Find("GutsGauge").GetComponent<GutsGaugeController>(); //GutsゲージのUIを取得
    }

    // Update is called once per frame
    void Update()
    {
        Move(); //移動を実行
        Attack(); //攻撃を実行
    }

    private void Move() //オブジェクトの移動を実行するメソッド
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

        if (-9 <= this.transform.position.x + move && this.transform.position.x + move <= 9) //移動が制限範囲内に収まっているかを判定
        {
            this.transform.Translate(move, 0, 0); //プレイヤーの移動を反映
        }
    }

    private void Attack() //オブジェクトの攻撃を実行するメソッド
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) //UpArrowを押した場合
        {
            UpArrowKey.Invoke(this, Projectiles[0]);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))  //LeftArrowを押した場合
        {
            //LeftArrowKey.Invoke(this, );
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))  //DownArrowを押した場合
        {
            //DownArrowKey.Invoke(this, );
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) //RightArrowを押した場合
        {
            //RightArrowKey.Invoke(this, );
        }
    }

    //ProjectilePrefabからProjectileオブジェクトを生成
    public GameObject InstanciateProjectile() //ProjectileEventクラスのThrowProjectileメソッドから呼ばれる
    {
        return Instantiate(ProjectilePrefab, this.transform.position, Quaternion.identity);
    }

    public void UpdateHpUI(float hp) //HpゲージのUIを更新
    {
        HpGauge.Value = hp;
    }

    public void UpdateGutsUI(float guts) //GutsゲージのUIを更新
    {
        GutsGauge.Value = guts;
    }
}
