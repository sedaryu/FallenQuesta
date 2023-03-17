using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Player Player { get; private set; } //未実装
    public PlayerModel PlayerModel { get; private set; }
    public GameObject PlayerObject { get; private set; }
    public PlayerPlayerControllerPresender PPCPresender { get; private set; }

    private PlayerGenerator playerGenerator; //プレイヤープレハブからプレイヤーオブジェクトを生成するためのクラス

    private void Awake()
    {
        Player = new Player("Sworder", 10.0f); //各プレイヤーキャラのデータはもっと適切な方法で管理する予定
        playerGenerator = GetComponent<PlayerGenerator>(); //あらかじめゲームコントローラーにアタッチされてる
        GeneratePlayer(Player);


    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GeneratePlayer(Player player)
    {
        PlayerModel = new PlayerModel(player); //
        PlayerObject = playerGenerator.Generate(player); //スピードやスプライトなどのデータをplayerを参照し設定される
        PPCPresender = new PlayerPlayerControllerPresender(PlayerModel, PlayerObject.GetComponent<PlayerController>());
    }
}
