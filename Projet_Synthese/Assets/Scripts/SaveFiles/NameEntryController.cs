using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Contrôleur pour la saisie du nom du joueur.
/// Auteur(s): Maxime
/// </summary>
public class NameEntryController : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    private SaveSystem saveSystem = new SaveSystem();

    private int levelsCompleted = 0;
    public void OnConfirmButtonClicked()
    {
        string playerName = playerNameInput.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            int selectedSlot = PlayerPrefs.GetInt("SelectedSaveSlot");
            PlayerPrefs.SetString("CurrentPlayerName", playerName);
            if (playerName == "Sl1mmy" || playerName == "Veryba")
            {
                levelsCompleted = 6;
            }
            saveSystem.SaveGame(selectedSlot, playerName, levelsCompleted);
        }
    }
}
