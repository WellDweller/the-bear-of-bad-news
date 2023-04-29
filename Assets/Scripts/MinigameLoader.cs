using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;



public class MinigameLoader : MonoBehaviour
{
    public MinigameResult LastResult { get; private set; }

    // unity events allow wiring up stuff in the editor
    [field:SerializeField] public UnityEvent<Minigame> OnMinigameLoad { get; private set; }

    [field:SerializeField] public UnityEvent<Minigame> OnMinigameEnd { get; private set; }

    [SerializeField] string testSceneToLoad;

    [SerializeField] List<string> minigameScenes;

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



    public void LoadRandomMinigame()
    {
        var sceneName = minigameScenes[Random.Range(0, minigameScenes.Count)];
        LoadMinigameScene(sceneName);
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

        SongManager.Instance.PlayMinigameSong();
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

        SongManager.Instance.PlayMainSong();
    }



    void HandleMinigameLoaded(Minigame game)
    {
        Debug.Log("Minigame loaded");
        OnMinigameLoad?.Invoke(game);
        game.OnEndGame.AddListener(HandleMinigameEnded);
    }

    void HandleMinigameUnloaded(Minigame game)
    {
        Debug.Log("Minigame unloaded");
        OnMinigameEnd?.Invoke(game);
        game.OnEndGame.RemoveListener(HandleMinigameEnded);
    }

    void HandleMinigameEnded(MinigameResult result)
    {
        Debug.Log($"Minigame result text: {result.text}");
        LastResult = result;
        UnloadMinigame();
    }
}
