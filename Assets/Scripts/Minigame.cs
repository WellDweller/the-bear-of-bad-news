using System;
using UnityEngine;
using UnityEngine.Events;



public class Minigame : MonoBehaviour
{
    // These static events used by the minigame loader
    public static event Action<Minigame> OnMinigameLoaded;
    
    public static event Action<Minigame> OnMinigameUnloaded;

    // unity events allow wiring up stuff in the editor
    [field:SerializeField] public UnityEvent OnEndGame { get; private set; }



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
        OnEndGame?.Invoke();
    }
}