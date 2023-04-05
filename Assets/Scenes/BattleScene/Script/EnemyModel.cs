using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//EnemyModelクラスについての解説
//プロパティに代入されたエネミーの各ステータスを管理するクラス
//コンストラクターにおいて、引数として受け取ったEnemyクラスに代入されている各ステータスをプロパティに代入する

public class EnemyModel
{
    public string Name { get; private set; } //Enemyの名前
    public float Hp { get; private set; } //Hp(0以下になると死亡)
    public float MaxHp { get; private set; } //最大Hp
    public float Heal { get; private set; } //一秒あたりのHpのオート回復量
    public float Speed { get; private set; } //移動速度
    public float Power { get; private set; } //一秒間に放つProjectileの最大数
    public List<string> Projectiles { get; private set; } //使用するProjectileの名前

    public EnemyModel(Enemy enemy)
    {
        Name = enemy.Name;
        Hp = enemy.Hp;
        MaxHp = enemy.Hp;
        Heal = enemy.Heal;
        Speed = enemy.Speed;
        Power = enemy.Power;
        Projectiles = new List<string>();
    }

    public void DecreaseHp(float damage) //ダメージぶんHpが減少する
    { 
        Hp -= damage;
    }

    public void SelfHealing() //時間経過でHpが自動回復する
    {
        if (Hp <= MaxHp)
        {
            Hp += Heal * (Hp / MaxHp) * Time.deltaTime;
        }
    }
}
