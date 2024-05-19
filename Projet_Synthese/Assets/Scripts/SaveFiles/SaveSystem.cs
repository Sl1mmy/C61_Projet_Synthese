using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem
{
    private const int MaxSaveFiles = 3;

    // Save the game data to a specified slot (1, 2, or 3)
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

    // Load the game data from a specified slot (1, 2, or 3)
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


    // Get the file path for a specific save slot
    private string GetFilePath(int slot)
    {
        return Application.persistentDataPath + "/playerData" + slot + ".dat";
    }
}
