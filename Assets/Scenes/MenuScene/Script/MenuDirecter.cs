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

    //BattleScene�ɑ���Player��Enemy�̃L��������PlayerPrefs�ɃZ�b�g����
    //BattleScene�փV�[���J�ڂ��s��
    public void GameStartSetting()
    {
        List<string> enemyCharas = new List<string>();
        selectedEnemies.ForEach(x => enemyCharas.Add(x.SelectedCharaName()));
        enemyCharas.RemoveAll(x => x == "None"); //None(�I���Ȃ�)�����O����

        if (enemyCharas.Count < 1) //�Œ�ł��G�l�~�[�L��������̂͑I�����Ȃ���΂Ȃ�Ȃ�
        {
            return;
        }

        //PlayerPrefs.SetInt("EnemyCount", enemyCharas.Count); //�G�l�~�[�L�����̐����L�^

        selectedCharacters.Add("EnemyCount", enemyCharas.Count.ToString());

        for (int i = 0; i < enemyCharas.Count; i++)
        {
            //PlayerPrefs.SetString($"Enemies{i}", enemyCharas[i]); //�G�l�~�[�L���������L�^
            selectedCharacters.Add($"Enemies{i}", enemyCharas[i]);
        }

        //PlayerPrefs.SetString("Player", selectedPlayer.SelectedCharaName()); //�v���C���[�L���������L�^
        selectedCharacters.Add("Player", selectedPlayer.SelectedCharaName());

        //PlayerPrefs.Save();

        SceneManager.LoadScene("BattleScene"); //�V�[���J��
    }
}
