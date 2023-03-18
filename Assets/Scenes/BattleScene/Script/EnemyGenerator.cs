using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    public GameObject Generate(Enemy enemy)
    {
        GameObject enemyObject = Instantiate(enemyPrefab);
        enemyObject.GetComponent<EnemyController>().Constructor(enemy); //�X�s�[�h��X�v���C�g�Ȃǂ̃f�[�^��ݒ�
        return enemyObject;
    }
}
