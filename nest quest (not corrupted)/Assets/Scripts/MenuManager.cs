using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ContinueGame()
    {
        // Eventually load save state.
        SceneManager.LoadScene("SampleScene");
    }

    public void LoadGame()
    {
        // If no saves implemented, same as Continue.
        SceneManager.LoadScene("SampleScene");
    }

    public void Settings()
    {
        // Show settings panel
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
