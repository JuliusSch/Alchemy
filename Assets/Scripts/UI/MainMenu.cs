using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public LoadGameMenu LoadGameMenu;

    public void PlayGame()
    {
        if (GameDataManager.Instance.SelectedSave == null)
            GameDataManager.Instance.CreateNewSave();
        SceneManager.LoadScene("Game");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game Exited");
    }
}
