using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveControllerLevelSelect : MonoBehaviour
{
    public Button[] levelButtons; 
    private SaveSystem saveSystem = new SaveSystem();

    void Start()
    {

        int selectedSlot = PlayerPrefs.GetInt("SelectedSaveSlot");

        PlayerData data = saveSystem.LoadGame(selectedSlot);

        DisableLevels(data.levelCompleted);
    }

    void DisableLevels(int levelCompleted)
    {
        Button lastEnabledButton = null;
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i > levelCompleted)
            {
                levelButtons[i].interactable = false;
            }
            else
            {
                levelButtons[i].interactable = true;
                lastEnabledButton = levelButtons[i];
            }
        }
        if (lastEnabledButton != null)
        {
            ColorBlock colorBlock = lastEnabledButton.colors;
            colorBlock.normalColor = new Color32(208, 136, 255, 255);
            print(colorBlock.normalColor);
            print(lastEnabledButton.colors.normalColor);
            lastEnabledButton.colors = colorBlock;
        }
    }
}
