using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

/// <summary>
/// Représente les données d'un joueur, notamment son nom et le niveau qu'il a complété.
/// Auteur(s): Noé
/// </summary>

public class PlayerData
{
    public string playerName;
    public int levelCompleted;


    public PlayerData(string playerName, int levelCompleted)
    {
        this.playerName = playerName;
        this.levelCompleted = levelCompleted;
    }
}
