using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Classe de base pour les filtres de la vue de scène Unity, 
/// permettant de synchroniser les paramètres entre la vue de scène et la caméra principale.
/// Auteur(s): Noé
/// </summary>
public class SceneViewFilter : MonoBehaviour
{
#if UNITY_EDITOR
    bool hasChanged = false;

    public virtual void OnValidate()
    {
        hasChanged = true;
    }

    static SceneViewFilter()
    {
        SceneView.duringSceneGui += CheckMe;
    }

    /// <summary>
    /// Méthode statique appelée pour vérifier et synchroniser les filtres de la vue de scène avec la caméra principale.
    /// </summary>
    /// <param name="sv">La vue de scène à synchroniser.</param>
    static void CheckMe(SceneView sv)
    {
        if (Event.current.type != EventType.Layout)
            return;
        if (!Camera.main)
            return;
        // Obtenez une liste de tous les éléments de l'appareil photo principal qui doivent être synchronisés.
        SceneViewFilter[] cameraFilters = Camera.main.GetComponents<SceneViewFilter>();
        SceneViewFilter[] sceneFilters = sv.camera.GetComponents<SceneViewFilter>();

        // Voyons si les listes sont de longueurs différentes ou quelque chose comme ca.         
        // Si c'est le cas, nous détruisons simplement tous les filtres de scène et nous les recréons à partir de maincamera
        if (cameraFilters.Length != sceneFilters.Length)
        {
            Recreate(sv);
            return;
        }
        for (int i = 0; i < cameraFilters.Length; i++)
        {
            if (cameraFilters[i].GetType() != sceneFilters[i].GetType())
            {
                Recreate(sv);
                return;
            }
        }

        // QUELS filtres, ou leur ordre n'a pas changé.
        // Copie de tous les paramètres des filtres qui ont changé.
        for (int i = 0; i < cameraFilters.Length; i++)
            if (cameraFilters[i].hasChanged || sceneFilters[i].enabled != cameraFilters[i].enabled)
            {
                EditorUtility.CopySerialized(cameraFilters[i], sceneFilters[i]);
                cameraFilters[i].hasChanged = false;
            }
    }


    /// <summary>
    /// Méthode statique appelée pour recréer les filtres de la vue de scène en cas de besoin de synchronisation.
    /// </summary>
    /// <param name="sv">La vue de scène pour laquelle les filtres doivent être recréés.</param>
    static void Recreate(SceneView sv)
    {
        SceneViewFilter filter;
        while (filter = sv.camera.GetComponent<SceneViewFilter>())
            DestroyImmediate(filter);

        foreach (SceneViewFilter f in Camera.main.GetComponents<SceneViewFilter>())
        {
            SceneViewFilter newFilter = sv.camera.gameObject.AddComponent(f.GetType()) as SceneViewFilter;
            EditorUtility.CopySerialized(f, newFilter);
        }
    }
#endif
}