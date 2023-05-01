using System;
using UnityEngine;
using UnityEngine.Events;


public struct MinigameResult
{
    public string text;

    public int score;

    public int stage;

    public static MinigameResult operator +(MinigameResult a, MinigameResult b) {
        return new() {
            text = a.text + " " + b.text,
            score = a.score + b.score,
            stage = a.stage > b.stage ? a.stage : b.stage,
        };
    }
}


public class Minigame : MonoBehaviour
{
    // These static events used by the minigame loader
    public static event Action<Minigame> OnMinigameLoaded;
    
    public static event Action<Minigame> OnMinigameUnloaded;

    // unity events allow wiring up stuff in the editor
    [field:SerializeField] public UnityEvent<MinigameResult> OnEndGame { get; private set; }

    [field:SerializeField] public UnityEvent OnCompleteGame { get; private set; }

    [field:SerializeField] public UnityEvent<MinigameResult> OnMinigameStageResult;

    protected MinigameResult result;

    protected bool IsStarted { get; private set; }

    protected bool IsComplete { get; private set; }


    void Awake()
    {
        OnMinigameLoaded?.Invoke(this);
    }

    void OnDestroy()
    {
        OnMinigameUnloaded?.Invoke(this);
    }

    public void StartGame()
    {
        IsStarted = true;
    }

    public void CompleteGame()
    {
        IsComplete = true;
        OnCompleteGame?.Invoke();
    }

    public void EndGame()
    {
        EndGame(result);
    }

    public void EndGame(MinigameResult result)
    {
        OnEndGame?.Invoke(result);
    }

    protected void CompleteStage(int stage, string text, int points)
    {
        MinigameResult partial = new() { text = text, score = points, stage = stage };
        OnMinigameStageResult?.Invoke(partial);
        result += partial;
    }
}
