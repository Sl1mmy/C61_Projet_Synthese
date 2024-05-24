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
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i > levelCompleted)
            {
                levelButtons[i].interactable = false;
            }
            else
            {
                levelButtons[i].interactable = true;

            }
        }
        if (levelButtons[levelCompleted] != null)
        {
            ColorBlock colorBlock = levelButtons[levelCompleted].colors;
            colorBlock.normalColor = new Color32(208, 136, 255, 255);
            levelButtons[levelCompleted].colors = colorBlock;
        }
    }
}
