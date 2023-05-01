using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] GameObject MenuContainer;
    [SerializeField] GameObject HowToPlay;

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

    public void HandleHowToPlayButton()
    {
        MenuContainer.SetActive(false);
        HowToPlay.SetActive(true);
        SongManager.Instance.PlaySFX("click");
    }

    public void HandleBackButton()
    {
        MenuContainer.SetActive(true);
        HowToPlay.SetActive(false);
        SongManager.Instance.PlaySFX("click");
    }
}
