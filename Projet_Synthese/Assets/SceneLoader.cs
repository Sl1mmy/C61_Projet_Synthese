using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadNewGame()
    {
        SceneManager.LoadScene("NewGame");
    }

    public void LoadCreateFile()
    {
        SceneManager.LoadScene("CreateFile");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoadOptions()
    {
        SceneManager.LoadScene("Options");
    }
}