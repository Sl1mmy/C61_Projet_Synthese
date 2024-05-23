using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Contrôle le fonctionnement du jeu, y compris la gestion des sauvegardes et l'affichage des données de sauvegarde.
/// Auteur(s): Noé
/// </summary>
public class GameController : MonoBehaviour
{
    private SaveSystem saveSystem = new SaveSystem();

    public TextMeshProUGUI saveSlot1Text;
    public TextMeshProUGUI saveSlot2Text;
    public TextMeshProUGUI saveSlot3Text;

    public TextMeshProUGUI saveSlot1Levels;
    public TextMeshProUGUI saveSlot2Levels;
    public TextMeshProUGUI saveSlot3Levels;

    public Button deleteSlot1Button;
    public Button deleteSlot2Button;
    public Button deleteSlot3Button;

    void Start()
    {
        LoadAndDisplaySaves();
    }

    /// <summary>
    /// Charge et affiche les données de sauvegarde.
    /// </summary>
    void LoadAndDisplaySaves()
    {
        DisplaySaveData(1, saveSlot1Text, saveSlot1Levels, deleteSlot1Button);
        DisplaySaveData(2, saveSlot2Text, saveSlot2Levels, deleteSlot2Button);
        DisplaySaveData(3, saveSlot3Text, saveSlot3Levels, deleteSlot3Button);
    }


    /// <summary>
    /// Affiche les données de sauvegarde pour un emplacement spécifié.
    /// </summary>
    /// <param name="slot">L'emplacement de sauvegarde.</param>
    /// <param name="slotText">Le composant TextMeshProUGUI pour afficher le nom du joueur.</param>
    /// <param name="slotLevel">Le composant TextMeshProUGUI pour afficher les niveaux complétés.</param>
    /// <param name="deleteButton">Le bouton pour supprimer la sauvegarde.</param>
    void DisplaySaveData(int slot, TextMeshProUGUI slotText, TextMeshProUGUI slotLevel, Button deleteButton)
    {
        PlayerData data = saveSystem.LoadGame(slot);
        if (data != null)
        {
            slotText.text = data.playerName;
            slotLevel.text = data.levelCompleted + "/6";
            deleteButton.interactable = true;
        }
        else
        {
            slotText.text = "Empty Slot";
            slotLevel.text = "?";
            deleteButton.interactable = false;
        }
    }

    /// <summary>
    /// Appelée lorsqu'un emplacement de sauvegarde est cliqué.
    /// </summary>
    /// <param name="slot">L'emplacement de sauvegarde cliqué.</param>
    public void OnSaveSlotClicked(int slot)
    {
        PlayerData data = saveSystem.LoadGame(slot);
        if (data == null)
        {
            PlayerPrefs.SetInt("SelectedSaveSlot", slot);
            SceneManager.LoadScene("CreateFile"); // Replace with the scene for name entry
        }
        else
        {
            PlayerPrefs.SetInt("SelectedSaveSlot", slot);
            SceneManager.LoadScene("levels"); // Replace with the level selection scene
        }
    }


    /// <summary>
    /// Supprime le fichier de sauvegarde pour un emplacement spécifié.
    /// </summary>
    /// <param name="slot">L'emplacement de sauvegarde à supprimer.</param>
    public void DeleteSaveFile(int slot)
    {
        saveSystem.DeleteSave(slot);
        DisplaySaveData(slot, GetSlotTextComponent(slot), GetSlotLevelsComponent(slot), GetDeleteButtonComponent(slot));
    }

    private TextMeshProUGUI GetSlotTextComponent(int slot)
    {
        switch (slot)
        {
            case 1: return saveSlot1Text;
            case 2: return saveSlot2Text;
            case 3: return saveSlot3Text;
            default: return null;
        }
    }

    private TextMeshProUGUI GetSlotLevelsComponent(int slot)
    {
        switch (slot)
        {
            case 1: return saveSlot1Levels;
            case 2: return saveSlot2Levels;
            case 3: return saveSlot3Levels;
            default: return null;
        }
    }

    private Button GetDeleteButtonComponent(int slot)
    {
        switch (slot)
        {
            case 1: return deleteSlot1Button;
            case 2: return deleteSlot2Button;
            case 3: return deleteSlot3Button;
            default: return null;
        }
    }
}
