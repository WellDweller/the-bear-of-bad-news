using UnityEngine;
using UnityEngine.SceneManagement;



public class SceneLoader : MonoBehaviour
{
    // just exists so that we can wire this up in the editoro
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
