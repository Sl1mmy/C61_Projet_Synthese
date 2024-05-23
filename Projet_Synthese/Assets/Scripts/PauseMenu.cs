using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Gère le menu de pause du jeu.
/// Auteur(s): Maxime
/// </summary>
public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject PlayerButton;
    public Animator fadeAnimator;
    public Image black;

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
        Time.timeScale = 1; // Assurez-vous de remettre le temps à l'échelle normale
        StartCoroutine(FadeAndLoadScene("Levels"));
    }

    public void LoadQuit()
    {
        Time.timeScale = 1; // Assurez-vous de remettre le temps à l'échelle normale
        StartCoroutine(FadeAndLoadScene("Menu"));
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        // Démarre l'animation de fondu
        fadeAnimator.SetBool("Fade", true);

        // Attend que le fondu soit complet (black.color.a == 1)
        yield return new WaitUntil(() => black.color.a == 1);

        // Charge la nouvelle scène
        SceneManager.LoadScene(sceneName);

        // Réinitialise l'animation de fondu pour être prête pour la prochaine utilisation
        fadeAnimator.SetBool("Fade", false);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Assurez-vous que l'écran n'est pas noir lorsqu'une nouvelle scène est chargée
        fadeAnimator.SetBool("Fade", false);
        black.color = new Color(black.color.r, black.color.g, black.color.b, 0);
    }
}
