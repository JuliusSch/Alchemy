using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    public List<SaveData> Saves;

    private SaveData _selectedSave;
    private string _savesFileLocation;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        _savesFileLocation = Application.persistentDataPath + "/saves";
        LoadSaves();
    }

    public void LoadSaves()
    {
        Saves = new List<SaveData>();
        Directory.CreateDirectory(_savesFileLocation);                  // Create if it doesn't yet exist.
        var directories = Directory.GetDirectories(_savesFileLocation);
        foreach (var dir in directories)
        {
            if (File.Exists(dir + "/data.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                //string info;
                //using var streamReader = new StreamReader(dir + "/info.txt", Encoding.UTF8);
                //info = streamReader.ReadToEnd();
                FileStream file = File.Open(dir + "/data.dat", FileMode.Open);
                SaveData data = (SaveData)bf.Deserialize(file);
                file.Close();
                Saves.Add(data);
            }
        }
        if (Saves.Count > 0)
            SelectedSave = Saves.OrderBy(s => s.LastUpdatedTime).Last();
    }

    #region Properties

    public SaveData SelectedSave
    {
        get => _selectedSave ?? CreateNewSave();
        set => _selectedSave = value;
    }

    #endregion

    public void SaveGame(SaveData saveData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(_savesFileLocation + "/" + saveData.Name + "/data.dat");
        bf.Serialize(file, saveData);
        file.Close();
        //Debug.Log("Game data saved!");
    }

    public SaveData CreateNewSave()
    {
        SaveData newSave = new SaveData(Saves.Count);
        // Check name doesn't already exist or add popup window to select save name and ensure it's unique before continuing.
        Directory.CreateDirectory(_savesFileLocation + "/" + newSave.Name);
        Saves.Add(newSave);
        SaveGame(newSave);

        return SelectedSave = newSave;
    }

}
