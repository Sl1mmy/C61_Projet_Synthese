using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;


/// <summary>
/// Gère la fin d'un niveau et le chargement du niveau suivant.
/// Auteur(s): Maxime
/// </summary>
public class vaisseaufinish : MonoBehaviour
{
    public enum LevelNames { _lvl1, _lvl2, _lvl3, _lvl4, _lvl5, _lvl6 }

    public LevelNames nextLevel;
    public int currentLevel = 0;
    public float liftOffSpeed = 5f;
    public float liftOffDuration = 2f;
    public float waitBeforeLiftOff = 1f;

    private SaveSystem saveSystem = new SaveSystem();
    private bool isLiftOff = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isLiftOff)
        {
            isLiftOff = true;
            SaveGame(currentLevel);
            StartCoroutine(LiftOffAndLoadNextLevel(collision.gameObject));
        }
    }

    private IEnumerator LiftOffAndLoadNextLevel(GameObject player)
    {

        player.SetActive(false);
        yield return new WaitForSeconds(waitBeforeLiftOff);
        float elapsedTime = 0f;
        Vector3 initialPosition = transform.position;

        while (elapsedTime < liftOffDuration)
        {
            transform.position = initialPosition + Vector3.up * (liftOffSpeed * (elapsedTime / liftOffDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(nextLevel.ToString());
    }

    /// <summary>
    /// Sauvegarde le niveau actuel dans le système de sauvegarde du jeu.
    /// </summary>
    /// <param name="currentLevel">Le niveau actuel.</param>
    private void SaveGame(int currentLevel)
    {
        int selectedSlot = PlayerPrefs.GetInt("SelectedSaveSlot");
        string currentPlayerName = PlayerPrefs.GetString("CurrentPlayerName");

        PlayerData data = saveSystem.LoadGame(selectedSlot);
        if (data.levelCompleted < currentLevel)
        {
            saveSystem.SaveGame(selectedSlot, currentPlayerName, currentLevel);
        }
    }
}
