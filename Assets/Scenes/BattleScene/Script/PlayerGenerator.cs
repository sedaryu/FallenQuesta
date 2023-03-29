using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGenerator : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    public GameObject Generate(Player player)
    { 
        GameObject playerObject = Instantiate(playerPrefab);
        playerObject.GetComponent<PlayerController>().Constructor(player); //�e�X�e�[�^�X��Player���Q�Ƃ��ݒ�
        return playerObject;
    }
}
