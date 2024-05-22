using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;


/// <summary>
/// Gère la fin d'un niveau et le chargement du niveau suivant.
/// Auteur(s): Maxime
/// </summary>
public class vaisseaufinish : MonoBehaviour
{
    public enum LevelNames { _lvl1, _lvl2, _lvl3, _lvl4, _lvl5, _lvl6 }

    public LevelNames nextLevel;
    public int currentLevel = 0;

    private SaveSystem saveSystem = new SaveSystem();

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SaveGame(currentLevel);
            SceneManager.LoadScene(nextLevel.ToString());
        }
    }

    /// <summary>
    /// Sauvegarde le niveau actuel dans le système de sauvegarde du jeu.
    /// </summary>
    /// <param name="currentLevel">Le niveau actuel.</param>
    private void SaveGame(int currentLevel)
    {
        int selectedSlot = PlayerPrefs.GetInt("SelectedSaveSlot");
        string currentPlayerName = PlayerPrefs.GetString("CurrentPlayerName");

        PlayerData data = saveSystem.LoadGame(selectedSlot);
        if (data.levelCompleted < currentLevel) { 
            saveSystem.SaveGame(selectedSlot, currentPlayerName, currentLevel); 
        }
    }
}
