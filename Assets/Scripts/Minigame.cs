using System;
using UnityEngine;
using UnityEngine.Events;


public struct MinigameResult
{
    public string text;

    public int score;
}


public class Minigame : MonoBehaviour
{
    // These static events used by the minigame loader
    public static event Action<Minigame> OnMinigameLoaded;
    
    public static event Action<Minigame> OnMinigameUnloaded;

    // unity events allow wiring up stuff in the editor
    [field:SerializeField] public UnityEvent<MinigameResult> OnEndGame { get; private set; }

    protected MinigameResult result;

    // public string[,] dialog = { { "i'm sorry, your", "ur", "ya" }, { "husband", "mate", "boi" }, { "passed away peacefully", "ate it", "ded" } };
    public EncounterStage dialog;



    void Awake()
    {
        OnMinigameLoaded?.Invoke(this);
    }

    void OnDestroy()
    {
        OnMinigameUnloaded?.Invoke(this);
    }

    public void EndGame()
    {
        EndGame(result);
    }

    public void EndGame(MinigameResult result)
    {
        OnEndGame?.Invoke(result);
    }
}
