using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectController : MonoBehaviour
{
    public bool Player { get; private set; } //味方キャラか敵キャラか
    public List<string> Characters { get; private set; } //キャラ名を格納

    private int selectedChara = 0; //選択されているキャラのインデックス番号
    private SpriteRenderer characterImage; //選択されているキャラの画像を表示

    private Text characterName; //選択されているキャラの名前を表示

    //各キャラ名を代入
    public void SettingCharacter(List<string> characters)
    { 
        Characters = characters;
    }

    private void Start()
    {
        characterImage = this.GetComponent<SpriteRenderer>();
        characterName = this.transform.GetChild(0).gameObject.GetComponent<Text>();
        UpdateChara();
    }

    //選択キャラの表示を更新
    private void UpdateChara()
    {
        if (Player)
        {
            characterImage.sprite = Resources.Load($"Player/{Characters[selectedChara]}", typeof(Sprite)) as Sprite;
        }
        else
        {
            characterImage.sprite = Resources.Load($"Enemy/{Characters[selectedChara]}", typeof(Sprite)) as Sprite;
        }

        characterName.text = Characters[selectedChara];
    }

    //オブジェクトがクリックされると表示するキャラを変更
    public void ChangeChara()
    {
        selectedChara += 1;

        if (selectedChara >= Characters.Count)
        { 
            selectedChara = 0;
        }

        UpdateChara();
    }
}
