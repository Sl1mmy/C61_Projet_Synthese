using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


/// <summary>
/// Syst�me de sauvegarde pour sauvegarder et charger les donn�es du joueur.
/// </summary>
public class SaveSystem
{
    private const int MaxSaveFiles = 3;

    /// <summary>
    /// Sauvegarde les donn�es de jeu dans un emplacement sp�cifi� (1, 2 ou 3).
    /// </summary>
    /// <param name="slot">L'emplacement de sauvegarde.</param>
    /// <param name="playerName">Le nom du joueur.</param>
    /// <param name="levelsCompleted">Le nombre de niveaux termin�s par le joueur.</param>
    public void SaveGame(int slot, string playerName, int levelsCompleted)
    {
        if (slot < 1 || slot > MaxSaveFiles)
        {
            Debug.LogError("Invalid save slot number. Slot number must be between 1 and 3.");
            return;
        }

        PlayerData data = new PlayerData(playerName, levelsCompleted);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(GetFilePath(slot));
        bf.Serialize(file, data);
        file.Close();
    }

    /// <summary>
    /// Charge les donn�es de jeu depuis un emplacement sp�cifi� (1, 2 ou 3).
    /// </summary>
    /// <param name="slot">L'emplacement de sauvegarde.</param>
    /// <returns>Les donn�es du joueur charg�es.</returns>
    public PlayerData LoadGame(int slot)
    {
        if (slot < 1 || slot > MaxSaveFiles)
        {
            Debug.LogError("Invalid load slot number. Slot number must be between 1 and 3.");
            return null;
        }

        if (File.Exists(GetFilePath(slot)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(GetFilePath(slot), FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();
            return data;
        }
        else
        {
            Debug.LogWarning("No save file found in slot " + slot);
            return null;
        }
    }

    /// <summary>
    /// Supprime le fichier de sauvegarde pour un emplacement sp�cifi� (1, 2 ou 3).
    /// </summary>
    /// <param name="slot">L'emplacement de sauvegarde � supprimer.</param>
    public void DeleteSave(int slot)
    {
        if (slot < 1 || slot > MaxSaveFiles)
        {
            Debug.LogError("Invalid delete slot number. Slot number must be between 1 and 3.");
            return;
        }

        string filePath = GetFilePath(slot);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Save file in slot " + slot + " deleted.");
        }
        else
        {
            Debug.LogWarning("No save file found in slot " + slot + " to delete.");
        }
    }


    // Obtient le chemin du fichier pour un emplacement de sauvegarde sp�cifique
    private string GetFilePath(int slot)
    {
        return Application.persistentDataPath + "/playerData" + slot + ".dat";
    }
}
