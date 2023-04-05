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

public class Player
{
    public string Name { get; private set; } //Playerの名前
    public float Defense { get; private set; } //敵の攻撃が一発当たるにつきどれだけHpが減少するか、最小値は0.1、最大値は1
    public float Speed { get; private set; } //移動速度
    public float Recover { get; private set; } //一秒間に回復するガッツ量
    public List<string> Projectiles { get; private set; } = new List<string>(); //使用するProjectile名

    public Player(string name, float defense, float speed, float recover, List<string> projectiles)
    { 
        Name = name;
        Defense = defense;
        Speed = speed;
        Recover = recover;
        Projectiles = projectiles;
    }
}
