using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Contr�leur pour la saisie du nom du joueur.
/// Auteur(s): Maxime
/// </summary>
public class NameEntryController : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    private SaveSystem saveSystem = new SaveSystem();

    public void OnConfirmButtonClicked()
    {
        string playerName = playerNameInput.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            int selectedSlot = PlayerPrefs.GetInt("SelectedSaveSlot");
            PlayerPrefs.SetString("CurrentPlayerName", playerName);
            saveSystem.SaveGame(selectedSlot, playerName, 0);
        }
    }
}
