using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private Transform TargetTransform { get; set; } //Projectileのターゲットとなるオブジェクトの位置、当たり判定に用いる
    private List<Transform> TargetTransforms { get; set; } //ターゲットとなるオブジェクトが複数の場合はこちらに代入
    private ProjectileModel ProjectileModel { get; set; } //Projectileの移動、当たり判定などの処理を行う
    private bool Player { get; set; } //プレイヤーが放ったProjectileの場合true、エネミーの場合はfalse

    public event Action ProjectileHitPlayer; //エネミーのProjectileがプレイヤーに当たった場合発生
    public event Action<List<int>, float> ProjectileHitEnemy; //プレイヤーのProjectileがエネミーに当たった場合発生
                                                              //引数List<int>は当たったエネミーのインデックス番号
                                                              //引数floatはエネミーに与えるダメージ
    public void Constructor(List<Transform> targetTransforms, Projectile projectile)
    {
        TargetTransforms = targetTransforms;
        ProjectileModel = new ProjectileModel(projectile);
        Player = projectile.Player;
    }
    public void Constructor(Transform targetTransform, Projectile projectile)
    {
        TargetTransform = targetTransform;
        ProjectileModel = new ProjectileModel(projectile);
        Player = projectile.Player;
    }

    public void SetTargetTransforms(List<Transform> targetTransforms)
    {
        TargetTransforms = targetTransforms;
    }

    // Start is called before the first frame update
    void Start()
    {
        TransformControll(); //プレイヤーかエネミーか、どちらが放ったProjectileかでトランスフォームを調整
    }

    // Update is called once per frame
    void Update()
    {
        ProjectileFlying(); //Projectileを動かす

        ProjectileHittingTarget(); //当たり判定

        JudgeDestroy(); //一定範囲を超えたら破壊する
    }

    private void TransformControll()
    {
        if (Player)
        {
            this.transform.position = new Vector3(this.transform.position.x, -3.5f, 0);
            this.transform.localScale = new Vector3(1, 1, 1);
            this.transform.rotation = new Quaternion(0, 0, 0, 0);
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load($"Projectile/{ProjectileModel.Name}", typeof(Sprite)) as Sprite;
        }
        else
        {
            this.transform.position = new Vector3(this.transform.position.x, -1, 0);
            this.transform.localScale = new Vector3(0.2f, 0.2f, 1);
            this.transform.rotation = new Quaternion(180, 0, 0, 0);
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load($"Projectile/{ProjectileModel.Name}", typeof(Sprite)) as Sprite;
        }
    }

    private void ProjectileFlying()
    {
        float[] move = new float[3];
        move = ProjectileModel.Fly(); //ProjectileModelで移動計算を行う
        this.transform.Translate(0, move[0] * Time.deltaTime, 0); //位置を更新
        this.transform.Rotate(move[1] * Time.deltaTime, 0, 0); //角度を更新
        this.transform.localScale += new Vector3(move[2] * Time.deltaTime, move[2] * Time.deltaTime, 0); //大きさを更新

        ProjectileModel.UpdateTime(Time.deltaTime); //ProjectileModelに移動時間を渡す
    }

    private void ProjectileHittingTarget()
    {
        if (Player)
        {
            //ターゲットのトランスフォームからposition.x、lossyScale.xだけを抜き出す
            List<float> targets_x = TargetTransforms.Select(x => x.position.x).ToList();
            List<float> targets_scale = TargetTransforms.Select(x => x.lossyScale.x).ToList();
            //引数として関数へ渡す
            List<int> hits = ProjectileModel.JudgeHit(this.transform.position.x, targets_x, targets_scale);

            if (hits.Count > 0) //hitsの要素数が1以上であればヒット
            {
                ProjectileHitEnemy.Invoke(hits, ProjectileModel.Attack); //イベントを実行
                Destroy(gameObject);
            }
        }
        else
        {
            bool hit = ProjectileModel.JudgeHit(this.transform.position.x, this.transform.position.y, 
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
