using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerDataManager : MonoBehaviour {

    private Dictionary<string, bool> createdConcoctions;
    private Dictionary<string, bool> discoveredIngredients;
    [SerializeField] private AlchemistBook book;

    private void Start() {
        bool loaded = loadGame();
        if (!loaded) resetData();
    }

    public void addConcoction(ConcoctionSO concoction) {
        createdConcoctions[concoction.concoctionName] = true;
        book.updateEntryComplete(concoction, true);
    }

    public void saveGame() {
        //BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Create(Application.persistentDataPath + "/MySaveData.dat");
        //SaveData data = new SaveData();
        //data = assignData(data);
        //bf.Serialize(file, data);
        //file.Close();
        //Debug.Log("Game data saved!");
    }

    private SaveData assignData(SaveData data) {
        data.createdConcoctions = createdConcoctions;
        data.discoveredIngredients = discoveredIngredients;
        return data;
    }

    private void retrieveData(SaveData data) {
        createdConcoctions = data.createdConcoctions;
        discoveredIngredients = data.discoveredIngredients;
    }

    private void resetData() {
        createdConcoctions = new Dictionary<string, bool>();
        createdConcoctions.Add("Resistance to Burning", false);
        discoveredIngredients = new Dictionary<string, bool>();
    }

    public bool loadGame() {
        if (File.Exists(Application.persistentDataPath + "/MySaveData.dat")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/MySaveData.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            retrieveData(data);
            Debug.Log("Game data loaded!");
            return true;
        } else {
            Debug.LogWarning("There is no save data!");
            return false;
        }
    }

    public void deleteSave() {
        if (File.Exists(Application.persistentDataPath + "/MySaveData.dat")) {
            File.Delete(Application.persistentDataPath + "/MySaveData.dat");
            resetData();
            Debug.Log("Data reset complete!");
        } else Debug.LogError("No save data to delete.");
    }

}