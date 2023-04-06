using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//ProjectileControllerクラスについての解説
//シーン上での飛び道具オブジェクトの動作を管理するクラス
//飛び道具オブジェクトがProjectilePrefabからインスタンスされると同時に、Constructorメソッドが実行され、
//引数として受け取ったProjectileクラスに代入されている各ステータスを、ProjectileCalculatorクラスのプロパティに代入する
//同時にターゲットとなるキャラクターのTransformも取得する
//

public class ProjectileController : MonoBehaviour
{
    //インスペクターから代入する
    [SerializeField] private string _name;
    [SerializeField] private bool _player;
    [SerializeField] private float _attack;
    [SerializeField] private float _cost;
    [SerializeField] private float _flyingTime;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotation;
    [SerializeField] private float _scale;

    public string Name { get => _name; private set => _name = value; }
    public bool Player { get => _player; private set => _player = value; } //プレイヤーが放ったProjectileの場合true、エネミーの場合はfalse
    public float Attack { get => _attack; private set => _attack = value; }
    public float Cost { get => _cost; private set => _cost = value; }
    public float FlyingTime { get => _flyingTime; private set => _flyingTime = value; }
    public float Speed { get => _speed; private set => _speed = value; }
    public float Rotation { get => _rotation; private set => _rotation = value; }
    public float Scale { get => _scale; private set => _scale = value; }

    private Transform TargetTransform { get; set; } //Projectileのターゲットとなるオブジェクトの位置、当たり判定に用いる
    private List<Transform> TargetTransforms { get; set; } //ターゲットとなるオブジェクトが複数の場合はこちらに代入
    private ProjectileCalculator ProjectileCalc { get; set; } //Projectileの移動、当たり判定などの計算処理を行う

    public event Action ProjectileHitPlayer; //エネミーのProjectileがプレイヤーに当たった場合発生

    //プレイヤーのProjectileがエネミーに当たった場合発生
    //引数List<int>は当たったエネミーのインデックス番号
    //引数floatはエネミーに与えるダメージ
    public event Action<List<int>, float> ProjectileHitEnemy;

    //ターゲットのTransformを取得
    public void Constructor(List<Transform> targetTransforms) //ターゲットが複数の場合
    {
        TargetTransforms = targetTransforms;
    }
    public void Constructor(Transform targetTransform) //ターゲットが単独の場合
    {
        TargetTransform = targetTransform;
    }

    public void SetTargetTransforms(List<Transform> targetTransforms) //ターゲットを変更したい場合
    {
        TargetTransforms = targetTransforms;
    }

    // Start is called before the first frame update
    void Start()
    {
        ProjectileCalc = new ProjectileCalculator(Player, FlyingTime, Speed, Rotation, Scale);
        SettingTransform(); //プレイヤーかエネミーか、どちらが放ったProjectileかでTransformを調整
    }

    // Update is called once per frame
    void Update()
    {
        ProjectileFlying(); //Projectileを動かす

        ProjectileHittingTarget(); //当たり判定

        JudgeDestroy(); //一定範囲を超えたら破壊する
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
        move = ProjectileCalc.Fly(); //ProjectileCalcで移動計算を行う
        this.transform.Translate(0, move[0] * Time.deltaTime, 0); //位置を更新
        this.transform.Rotate(move[1] * Time.deltaTime, 0, 0); //角度を更新
        this.transform.localScale += new Vector3(move[2] * Time.deltaTime, move[2] * Time.deltaTime, 0); //大きさを更新

        ProjectileCalc.UpdateTime(Time.deltaTime); //ProjectileCalcに移動時間を渡す
    }

    private void ProjectileHittingTarget()
    {
        if (Player)
        {
            //ターゲットのトランスフォームからposition.x、lossyScale.xだけを抜き出す
            List<float> targets_x = TargetTransforms.Select(x => x.position.x).ToList();
            List<float> targets_scale = TargetTransforms.Select(x => x.lossyScale.x).ToList();
            //引数として関数へ渡す
            List<int> hits = ProjectileCalc.JudgeHit(this.transform.position.x, targets_x, targets_scale);

            if (hits.Count > 0) //hitsの要素数が1以上であればヒット
            {
                ProjectileHitEnemy.Invoke(hits, Attack); //イベントを実行
                Destroy(gameObject);
            }
        }
        else
        {
            bool hit = ProjectileCalc.JudgeHit(this.transform.position.x, this.transform.position.y, 
                                                this.transform.lossyScale.x * 0.5f, this.transform.lossyScale.y * 0.5f, 
                                                TargetTransform.position.x, TargetTransform.lossyScale.x * 0.5f);
            if (hit) //エネミーのプロジェクティルがプレイヤーに当たった場合、hitはtrue
            {
                ProjectileHitPlayer.Invoke(); //イベントを実行
                Destroy(gameObject);
            }
        }
    }

    private void JudgeDestroy() //一定範囲を超えた場合破壊する
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
