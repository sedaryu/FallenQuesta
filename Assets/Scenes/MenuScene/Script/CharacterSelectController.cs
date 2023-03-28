using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectController : MonoBehaviour
{
    public bool Player { get; private set; } //�����L�������G�L������
    public List<string> Characters { get; private set; } //�L���������i�[

    private int selectedChara = 0; //�I������Ă���L�����̃C���f�b�N�X�ԍ�
    private SpriteRenderer characterImage; //�I������Ă���L�����̉摜��\��

    private Text characterName; //�I������Ă���L�����̖��O��\��

    //�e�L����������
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

    //�I���L�����̕\�����X�V
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
}
