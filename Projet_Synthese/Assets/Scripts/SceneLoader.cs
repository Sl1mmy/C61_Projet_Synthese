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

    public void LoadLevels()
    {
        SceneManager.LoadScene("Levels");
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("_lvl1");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("_lvl2");
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("_lvl3");
    }

    public void LoadLevel4()
    {
        SceneManager.LoadScene("_lvl4");
    }

    public void LoadLevel5()
    {
        SceneManager.LoadScene("_lvl5");
    }

    public void LoadLevel6()
    {
        SceneManager.LoadScene("_lvl6");
    }

}