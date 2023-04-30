using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string sceneName;

    public void HandlePlayButton()
    {
        SceneManager.LoadScene(sceneName);
        SongManager.Instance.PlaySFX("click");
    }

    public void HandleQuitButton()
    {
        SongManager.Instance.PlaySFX("click");
        Debug.Log("Quitting Application");
        Application.Quit();
    }
}
