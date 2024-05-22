using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Classe de base pour les filtres de la vue de sc�ne Unity, 
/// permettant de synchroniser les param�tres entre la vue de sc�ne et la cam�ra principale.
/// Auteur(s): No�
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
    /// M�thode statique appel�e pour v�rifier et synchroniser les filtres de la vue de sc�ne avec la cam�ra principale.
    /// </summary>
    /// <param name="sv">La vue de sc�ne � synchroniser.</param>
    static void CheckMe(SceneView sv)
    {
        if (Event.current.type != EventType.Layout)
            return;
        if (!Camera.main)
            return;
        // Obtenez une liste de tous les �l�ments de l'appareil photo principal qui doivent �tre synchronis�s.
        SceneViewFilter[] cameraFilters = Camera.main.GetComponents<SceneViewFilter>();
        SceneViewFilter[] sceneFilters = sv.camera.GetComponents<SceneViewFilter>();

        // Voyons si les listes sont de longueurs diff�rentes ou quelque chose comme ca.         
        // Si c'est le cas, nous d�truisons simplement tous les filtres de sc�ne et nous les recr�ons � partir de maincamera
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

        // QUELS filtres, ou leur ordre n'a pas chang�.
        // Copie de tous les param�tres des filtres qui ont chang�.
        for (int i = 0; i < cameraFilters.Length; i++)
            if (cameraFilters[i].hasChanged || sceneFilters[i].enabled != cameraFilters[i].enabled)
            {
                EditorUtility.CopySerialized(cameraFilters[i], sceneFilters[i]);
                cameraFilters[i].hasChanged = false;
            }
    }


    /// <summary>
    /// M�thode statique appel�e pour recr�er les filtres de la vue de sc�ne en cas de besoin de synchronisation.
    /// </summary>
    /// <param name="sv">La vue de sc�ne pour laquelle les filtres doivent �tre recr��s.</param>
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