using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string sceneName;

    public void HandlePlayButton()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void HandleQuitButton()
    {
        Debug.Log("Quitting Application");
        Application.Quit();
    }
}
