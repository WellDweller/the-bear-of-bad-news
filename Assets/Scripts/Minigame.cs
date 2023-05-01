using System;
using System.Collections;
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
    static string[,] TEST_DIALOG = { { "your", "ur", "dat" }, { "husband", "mate", "boi" }, { "passed away", "ate it", "ded" } };

    // These static events used by the minigame loader
    public static event Action<Minigame> OnMinigameLoaded;
    
    public static event Action<Minigame> OnMinigameUnloaded;

    // unity events allow wiring up stuff in the editor
    [field:SerializeField] public UnityEvent<MinigameResult> OnEndGame { get; private set; }

    [field:SerializeField] public UnityEvent OnCompleteGame { get; private set; }

    [field:SerializeField] public UnityEvent<MinigameResult> OnMinigameStageResult;

    public bool IsPlaying => IsStarted && !IsComplete && !IsPaused;

    protected MinigameResult result;

    protected bool IsStarted { get; private set; }

    protected bool IsComplete { get; private set; }

    protected string[,] dialog { get; private set; }

    protected bool IsPaused { get; private set; }



    void Awake()
    {
        dialog = TEST_DIALOG;
        OnMinigameLoaded?.Invoke(this);
    }

    void OnDestroy()
    {
        OnMinigameUnloaded?.Invoke(this);
    }


    public void ConfigureGame(EncounterRoundData data)
    {
        /*
            data is like

                good [round1, round2, round3]
                med [round1, round2, round3]
                bad [round1, round2, round3]
            
            and sometimes these have 4 rounds instead of three.

            we want the data to be like

                round1 [good, med, bad]
                round2 [good, med, bad]
                round3 [good, med, bad]

            and to only ever have 3 rounds
        */

        string[,] gameData = new string[3, 3];
        for (int round = 0; round < 3; round++)
        {
            gameData[round, 0] = data.responses.good[round];
            gameData[round, 1] = data.responses.med[round];
            gameData[round, 2] = data.responses.bad[round];
        }

        if (data.responses.good.Count > 3)
            gameData[2, 0] += data.responses.good[3];
        if (data.responses.med.Count > 3)
            gameData[2, 1] += data.responses.med[3];
        if (data.responses.bad.Count > 3)
            gameData[2, 2] += data.responses.bad[3];

        dialog = gameData;
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

    public void PauseForDuration(float duration)
    {
        StartCoroutine(_PauseForDuration(duration));
    }
    protected void CompleteStage(int stage, string text, int points)
    {
        MinigameResult partial = new() { text = text, score = points, stage = stage };
        OnMinigameStageResult?.Invoke(partial);
        result += partial;
    }

    IEnumerator _PauseForDuration(float duration)
    {
        IsPaused = true;
        yield return new WaitForSeconds(duration);
        IsPaused = false;
    }
}
