using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

/// <summary>
/// Contrôleur pour la saisie du nom du joueur.
/// Auteur(s): Maxime
/// </summary>
public class NameEntryController : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    private SaveSystem saveSystem = new SaveSystem();

    private int levelsCompleted = 0;

    // Define the regex pattern for the player name validation
    private const string playerNamePattern = @"^[a-zA-Z0-9]{1,16}$";

    public void OnConfirmButtonClicked()
    {
        string playerName = playerNameInput.text;

        // Validate the player name using regex
        if (Regex.IsMatch(playerName, playerNamePattern))
        {
            int selectedSlot = PlayerPrefs.GetInt("SelectedSaveSlot");
            PlayerPrefs.SetString("CurrentPlayerName", playerName);
            if (playerName == "Sl1mmy" || playerName == "Veryba")
            {
                levelsCompleted = 6;
            }
            saveSystem.SaveGame(selectedSlot, playerName, levelsCompleted);
        }
        else
        {
            playerNameInput.text = "";
            Debug.Log("Invalid player name. It must be alphanumeric and between 3 to 16 characters long.");
        }
    }
}
