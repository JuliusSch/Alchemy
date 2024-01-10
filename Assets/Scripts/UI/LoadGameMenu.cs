using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadGameMenu : MonoBehaviour
{
    public List<GameObject> SaveSlots;
    public GameObject SaveUIPrefab, SavesPanel;

    private List<GameObject> buttons;

    private void Start()
    {
        LoadSaveSlots();
    }

    public void LoadSaveSlots()
    {
        buttons = new List<GameObject>();
        foreach (var save in GameDataManager.Instance.Saves)
        {
            AddSlot(save, GameDataManager.Instance.Saves.IndexOf(save));
        }
        if (GameDataManager.Instance.Saves.Count > 0)
            SwitchSelectedSlot(0);
    }

    public void SwitchSelectedSlot(int index)
    {
        GameDataManager.Instance.SelectedSave = GameDataManager.Instance.Saves[index];
        buttons[index].GetComponent<Button>().Select();
    }

    private void AddSlot(SaveData data, int index)
    {
        GameObject newSaveButtonItem = Instantiate(SaveUIPrefab);
        buttons.Add(newSaveButtonItem);

        newSaveButtonItem.transform.SetParent(SavesPanel.transform, false);

        newSaveButtonItem.GetComponentInChildren<TMP_Text>().text = data.Name + "\n" + data.LastUpdatedTime.ToString();
        newSaveButtonItem.GetComponent<Button>().onClick.AddListener(delegate { SwitchSelectedSlot(index); });
    }

    public void CreateNewGame()
    {
        var newSave = GameDataManager.Instance.CreateNewSave();
        AddSlot(newSave, GameDataManager.Instance.Saves.IndexOf(newSave));
    }
}
