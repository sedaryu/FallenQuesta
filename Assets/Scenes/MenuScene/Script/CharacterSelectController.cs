using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CharacterSelectController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private bool Player; //�����L�������G�L������
    [SerializeField] private List<string> Characters = new List<string>(); //�L���������i�[

    [NonSerialized] private int selectedChara = 0; //�I������Ă���L�����̃C���f�b�N�X�ԍ�
    private Image characterImage; //�I������Ă���L�����̉摜��\��

    private Text characterName; //�I������Ă���L�����̖��O��\��

    //�e�L����������
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

    //�I���L�����̕\�����X�V
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

    //�I�u�W�F�N�g���N���b�N�����ƕ\������L������ύX
    public void ChangeChara()
    {
        selectedChara += 1;

        if (selectedChara >= Characters.Count)
        { 
            selectedChara = 0;
        }

        UpdateChara();
    }

    //�I������Ă���L��������String��Ԃ�
    public string SelectedCharaName()
    {
        return Characters[selectedChara];
    }
}
