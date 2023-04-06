using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuDirecter : MonoBehaviour
{
    private CharacterSelectController selectedPlayer;
    private List<CharacterSelectController> selectedEnemies = new List<CharacterSelectController>();

    static public Dictionary<string, string> selectedCharacters;

    // Start is called before the first frame update
    void Start()
    {
        selectedPlayer = GameObject.Find("SelectedPlayer").GetComponent<CharacterSelectController>();
        selectedEnemies.Add(GameObject.Find("SelectedEnemy1").GetComponent<CharacterSelectController>());
        selectedEnemies.Add(GameObject.Find("SelectedEnemy2").GetComponent<CharacterSelectController>());
        selectedEnemies.Add(GameObject.Find("SelectedEnemy3").GetComponent<CharacterSelectController>());

        selectedCharacters = new Dictionary<string, string>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //BattleSceneに送るPlayerとEnemyのキャラ名をPlayerPrefsにセットする
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

        //PlayerPrefs.SetInt("EnemyCount", enemyCharas.Count); //エネミーキャラの数を記録

        selectedCharacters.Add("EnemyCount", enemyCharas.Count.ToString());

        for (int i = 0; i < enemyCharas.Count; i++)
        {
            //PlayerPrefs.SetString($"Enemies{i}", enemyCharas[i]); //エネミーキャラ名を記録
            selectedCharacters.Add($"Enemies{i}", enemyCharas[i]);
        }

        //PlayerPrefs.SetString("Player", selectedPlayer.SelectedCharaName()); //プレイヤーキャラ名を記録
        selectedCharacters.Add("Player", selectedPlayer.SelectedCharaName());

        //PlayerPrefs.Save();

        SceneManager.LoadScene("BattleScene"); //シーン遷移
    }
}
