using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Contr�le le fonctionnement du jeu, y compris la gestion des sauvegardes et l'affichage des donn�es de sauvegarde.
/// Auteur(s): No�
/// </summary>
public class GameController : MonoBehaviour
{
    private SaveSystem saveSystem = new SaveSystem();

    public TextMeshProUGUI saveSlot1Text;
    public TextMeshProUGUI saveSlot2Text;
    public TextMeshProUGUI saveSlot3Text;

    public Button deleteSlot1Button;
    public Button deleteSlot2Button;
    public Button deleteSlot3Button;

    void Start()
    {
        LoadAndDisplaySaves();
    }

    /// <summary>
    /// Charge et affiche les donn�es de sauvegarde.
    /// </summary>
    void LoadAndDisplaySaves()
    {
        DisplaySaveData(1, saveSlot1Text, deleteSlot1Button);
        DisplaySaveData(2, saveSlot2Text, deleteSlot2Button);
        DisplaySaveData(3, saveSlot3Text, deleteSlot3Button);
    }


    /// <summary>
    /// Affiche les donn�es de sauvegarde pour un emplacement sp�cifi�.
    /// </summary>
    /// <param name="slot">L'emplacement de sauvegarde.</param>
    /// <param name="slotText">Le composant TextMeshProUGUI pour afficher les donn�es de sauvegarde.</param>
    /// <param name="deleteButton">Le bouton pour supprimer la sauvegarde.</param>
    void DisplaySaveData(int slot, TextMeshProUGUI slotText, Button deleteButton)
    {
        PlayerData data = saveSystem.LoadGame(slot);
        if (data != null)
        {
            slotText.text = data.playerName;
            deleteButton.interactable = true;
        }
        else
        {
            slotText.text = "Empty Slot";
            deleteButton.interactable = false;
        }
    }

    /// <summary>
    /// Appel�e lorsqu'un emplacement de sauvegarde est cliqu�.
    /// </summary>
    /// <param name="slot">L'emplacement de sauvegarde cliqu�.</param>
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
    /// Supprime le fichier de sauvegarde pour un emplacement sp�cifi�.
    /// </summary>
    /// <param name="slot">L'emplacement de sauvegarde � supprimer.</param>
    public void DeleteSaveFile(int slot)
    {
        saveSystem.DeleteSave(slot);
        DisplaySaveData(slot, GetSlotTextComponent(slot), GetDeleteButtonComponent(slot));
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
