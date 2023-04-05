using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーキャラについての解説

//プレイヤーキャラのHpが0以下になればゲームオーバー
//Hpの最大値は各キャラ共通で1
//Defense(被ダメージ率)の値によって受けるダメージ量が変化する
//攻撃を一回行うたびGutsを消費する
//Gutsは時間経過で自動回復する
//Projectileごとに消費するGutsは異なる

//EnemyModelクラスについての解説
//プロパティに代入されたエネミーの各ステータスを管理するクラス
//コンストラクターにおいて、引数として受け取ったEnemyクラスに代入されている各ステータスをプロパティに代入する

public class PlayerModel
{
    public string Name { get; private set; } //Playerの名前
    public float Hp { get; private set; } = 1; //体力
    public float Defense { get; private set; } //一被弾あたりの被ダメージ量
    public float Speed { get; private set; } //移動速度
    public float Recover { get; private set; } //一秒間に回復するガッツ量

    float guts = 0;
    public float Guts //攻撃する際に消費するMPのようなステータス、時間経過で自動回復する
    {
        get => guts;
        //0以上1以下に値が収まるよう管理
        set
        {
            if (value < 0)
            {
                guts = 0;
            }
            else if (value > 1)
            {
                guts = 1;
            }
            else
            {
                guts = value;
            }
        }
    }

    public PlayerModel(Player player)
    { 
        Name = player.Name;
        Defense = player.Defense;
        Speed = player.Speed;
        Recover = player.Recover;
    }

    public bool DecreaseGuts(float guts) //渡されたぶんのガッツがあればガッツを消費しtrueを返す、無ければ消費せずfalseを返す
    {
        if (Guts - guts < 0)
        {
            return false;
        }

        Guts -= guts;
        return true;
    }

    public void RecoverGuts() //時間経過で自動的にGutsを回復させるメソッド
    {
        Guts += Recover * Time.deltaTime;
    }

    public void DecreaseHp() //Hpを減少させるメソッド
    {
        Hp -= Defense;
    }
}
