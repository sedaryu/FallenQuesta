using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGenerator : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    public GameObject Generate(Player player)
    { 
        GameObject playerObject = Instantiate(playerPrefab);
        playerObject.GetComponent<PlayerController>().Constructor(player); //�X�s�[�h��X�v���C�g�Ȃǂ̃f�[�^��ݒ�
        return playerObject;
    }
}
