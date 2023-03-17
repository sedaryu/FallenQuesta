using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Player Player { get; private set; } //������
    public PlayerModel PlayerModel { get; private set; }
    public GameObject PlayerObject { get; private set; }
    public PlayerPlayerControllerPresender PPCPresender { get; private set; }

    private PlayerGenerator playerGenerator; //�v���C���[�v���n�u����v���C���[�I�u�W�F�N�g�𐶐����邽�߂̃N���X

    private void Awake()
    {
        Player = new Player("Sworder", 10.0f); //�e�v���C���[�L�����̃f�[�^�͂����ƓK�؂ȕ��@�ŊǗ�����\��
        playerGenerator = GetComponent<PlayerGenerator>(); //���炩���߃Q�[���R���g���[���[�ɃA�^�b�`����Ă�
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
        PlayerObject = playerGenerator.Generate(player); //�X�s�[�h��X�v���C�g�Ȃǂ̃f�[�^��player���Q�Ƃ��ݒ肳���
        PPCPresender = new PlayerPlayerControllerPresender(PlayerModel, PlayerObject.GetComponent<PlayerController>());
    }
}
