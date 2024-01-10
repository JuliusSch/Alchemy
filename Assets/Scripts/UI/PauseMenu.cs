using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused = false;
    public GameObject PauseMenuUI;

    public void Start()
    {
        LoadGame();
    }

    void Update()
    {
        if (Input.GetButtonDown("Menu"))
        {
            if (IsPaused) Resume();
            else Pause();
        }
    }

    private void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ExitGame()
    {
        Resume();
        SaveGame();
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenu");
    }

    private void LoadGame()
    {
        // load from file here?
        var saveableObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>().ToList();
        saveableObjects.ForEach(obj => obj.Load(GameDataManager.Instance.SelectedSave));
    }

    private void SaveGame()
    {
        var saveableObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>().ToList();
        saveableObjects.ForEach(obj => obj.Save(GameDataManager.Instance.SelectedSave));
        GameDataManager.Instance.SelectedSave.LastUpdatedTime = DateTime.Now;
        GameDataManager.Instance.SaveGame(GameDataManager.Instance.SelectedSave);
    }
}
