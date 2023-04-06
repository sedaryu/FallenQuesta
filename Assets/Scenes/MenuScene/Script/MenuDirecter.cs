using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuDirecter : MonoBehaviour
{
    private CharacterSelectController selectedPlayer;
    private List<CharacterSelectController> selectedEnemies = new List<CharacterSelectController>();

    // Start is called before the first frame update
    void Start()
    {
        selectedPlayer = GameObject.Find("SelectedPlayer").GetComponent<CharacterSelectController>();
        selectedEnemies.Add(GameObject.Find("SelectedEnemy1").GetComponent<CharacterSelectController>());
        selectedEnemies.Add(GameObject.Find("SelectedEnemy2").GetComponent<CharacterSelectController>());
        selectedEnemies.Add(GameObject.Find("SelectedEnemy3").GetComponent<CharacterSelectController>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //BattleSceneに送るPlayerとEnemyのキャラ名をJsonファイルに記録する
    //BattleSceneへシーン遷移を行う
    public void GameStartSetting()
    {
        List<string> enemyCharas = new List<string>();
        selectedEnemies.ForEach(x => enemyCharas.Add(x.SelectedCharaName()));
        enemyCharas.RemoveAll(x => x == "None"); //None(選択なし)を除外する

        if (enemyCharas.Count < 1) //最低でもエネミーキャラを一体は選択しなければならない
        {
            return;
        }

        SelectedCharacter selected = new SelectedCharacter(selectedPlayer.SelectedCharaName(), enemyCharas);
        string json = JsonUtility.ToJson(selected);
        StreamWriter writer = new StreamWriter(Application.dataPath + "/StreamingAssets/JsonData/SelectedCharacter.json", false);
        writer.WriteLine(json);
        writer.Close();

        SceneManager.LoadScene("BattleScene"); //シーン遷移
    }
}
