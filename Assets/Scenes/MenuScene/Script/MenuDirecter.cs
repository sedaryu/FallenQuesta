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

    //BattleScene�ɑ���Player��Enemy�̃L��������Json�t�@�C���ɋL�^����
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

        SelectedCharacter selected = new SelectedCharacter(selectedPlayer.SelectedCharaName(), enemyCharas);
        string json = JsonUtility.ToJson(selected);
        StreamWriter writer = new StreamWriter(Application.dataPath + "/StreamingAssets/JsonData/SelectedCharacter.json", false);
        writer.WriteLine(json);
        writer.Close();

        SceneManager.LoadScene("BattleScene"); //�V�[���J��
    }
}
