using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameLoader : MonoBehaviour
{
    [SerializeField] string testSceneToLoad;

    bool CanLoadMinigame => loadedMinigame == null && loadedMinigame == null;

    Scene loadedMinigameScene;
    Minigame loadedMinigame;


    void Awake()
    {
        Minigame.OnMinigameLoaded += HandleMinigameLoaded;
        Minigame.OnMinigameUnloaded += HandleMinigameUnloaded;

        if (testSceneToLoad != "")
            LoadMinigameScene(testSceneToLoad);
    }

    void OnDestroy()
    {
        Minigame.OnMinigameLoaded -= HandleMinigameLoaded;
        Minigame.OnMinigameUnloaded -= HandleMinigameUnloaded;
    }



    // scene must be added to the build config, or SceneManager won't be able to load it
    public void LoadMinigameScene(string sceneName)
    {
        if (loadedMinigameScene != default(Scene))
            throw new System.Exception("Can't load a minigame when one is already loaded!");
        if (!CanLoadMinigame)
            throw new System.Exception("Tried to load a minigame scene before loader is ready");

        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        loadedMinigameScene = SceneManager.GetSceneByName(sceneName);
    }

    void UnloadMinigame()
    {
        if (loadedMinigameScene == null)
            throw new System.Exception("No minigame scene is loaded");
        if (!loadedMinigameScene.isLoaded)
            throw new System.Exception("Minigame scene was already unloaded");

        loadedMinigame = null;
        SceneManager.UnloadSceneAsync(loadedMinigameScene);
        loadedMinigameScene = default(Scene);
    }



    void HandleMinigameLoaded(Minigame game)
    {
        Debug.Log("Minigame loaded");

        game.OnEndGame.AddListener(HandleMinigameEnded);
    }

    void HandleMinigameUnloaded(Minigame game)
    {
        Debug.Log("Minigame unloaded");

        game.OnEndGame.RemoveListener(HandleMinigameEnded);
    }

    void HandleMinigameEnded()
    {
        UnloadMinigame();
    }
}
