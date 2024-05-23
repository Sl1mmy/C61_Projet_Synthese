using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// G�re le menu de pause du jeu.
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
        Time.timeScale = 1; // Assurez-vous de remettre le temps � l'�chelle normale
        StartCoroutine(FadeAndLoadScene("Levels"));
    }

    public void LoadQuit()
    {
        Time.timeScale = 1; // Assurez-vous de remettre le temps � l'�chelle normale
        StartCoroutine(FadeAndLoadScene("Menu"));
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        // D�marre l'animation de fondu
        fadeAnimator.SetBool("Fade", true);

        // Attend que le fondu soit complet (black.color.a == 1)
        yield return new WaitUntil(() => black.color.a == 1);

        // Charge la nouvelle sc�ne
        SceneManager.LoadScene(sceneName);

        // R�initialise l'animation de fondu pour �tre pr�te pour la prochaine utilisation
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
        // Assurez-vous que l'�cran n'est pas noir lorsqu'une nouvelle sc�ne est charg�e
        fadeAnimator.SetBool("Fade", false);
        black.color = new Color(black.color.r, black.color.g, black.color.b, 0);
    }
}
