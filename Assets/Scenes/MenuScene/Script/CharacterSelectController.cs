using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CharacterSelectController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private bool Player; //味方キャラか敵キャラか
    [SerializeField] private List<string> Characters = new List<string>(); //キャラ名を格納

    [NonSerialized] private int selectedChara = 0; //選択されているキャラのインデックス番号
    private Image characterImage; //選択されているキャラの画像を表示

    private Text characterName; //選択されているキャラの名前を表示

    //各キャラ名を代入
    public void SettingCharacter(List<string> characters)
    { 
        Characters = characters;
    }

    private void Start()
    {
        characterImage = this.GetComponent<Image>();
        characterName = this.transform.GetChild(0).gameObject.GetComponent<Text>();
        UpdateChara();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ChangeChara();
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
            if (selectedChara == 0)
            {
                characterImage.sprite = null;
            }
            else
            { 
                characterImage.sprite = Resources.Load($"Enemy/{Characters[selectedChara]}", typeof(Sprite)) as Sprite;
            }
            
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

    //選択されているキャラ名のStringを返す
    public string SelectedCharaName()
    {
        return Characters[selectedChara];
    }
}
