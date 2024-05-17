using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class vaisseaufinish : MonoBehaviour
{
    public enum LevelNames { _lvl1, _lvl2, _lvl3, _lvl4, _lvl5, _lvl6 }

    public LevelNames nextLevel;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextLevel.ToString());
        }
    }
}
