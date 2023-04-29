using UnityEngine;
using UnityEngine.SceneManagement;



public class SceneInitializer : MonoBehaviour
{
    public static SceneInitializer Instance { get; private set; }

    [field:SerializeField] public bool IsInitialized { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(this);
            return;
        }

        Instance = this;

        if (IsInitialized)
            return;

        SceneManager.LoadScene("Initialization", LoadSceneMode.Additive);
        IsInitialized = true;
    }
}
