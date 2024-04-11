using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject PlayerButton;
    // Update is called once per frame

    public void Pause()
    {
        PausePanel.SetActive(true);
        PlayerButton.SetActive(false);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        PausePanel.SetActive(false);
        PlayerButton.SetActive(true);
        Time.timeScale = 1;
    }

    public void LoadLevels()
    {
        SceneManager.LoadScene("Levels");
    }
}
