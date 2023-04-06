using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SelectedCharacter
{
    public string selectedPlayer;
    public List<string> selectedEnemies;

    public SelectedCharacter(string player, List<string> enemies)
    { 
        selectedPlayer = player;
        selectedEnemies = enemies;
    }
}
