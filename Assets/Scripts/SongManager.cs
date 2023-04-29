using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;



public class SongManager : MonoBehaviour
{
    public static SongManager Instance { get; private set; }

    [field:SerializeField] public bool IsInitialized { get; private set; }

    public AudioSource mainSongAudioSource;

    [SerializeField] public List<AudioClip> miniGameAudioClips;
    private AudioSource miniGameAudioSource;

    void Start()
    {
    }

    public void PlayMainSong()
    {
        StopMinigameSong();

        if (!mainSongAudioSource.isPlaying)
        {
            mainSongAudioSource.Play();
        }
    }

    public void PauseMainSong()
    {
        if (mainSongAudioSource.isPlaying)
        {
            mainSongAudioSource.Pause();
        }
    }

    public void PlayMinigameSong()
    {
        PauseMainSong();

        int randomIndex = Random.Range(0, miniGameAudioClips.Count);
        AudioClip randomClip = miniGameAudioClips[randomIndex];
        miniGameAudioSource.clip = randomClip;
        miniGameAudioSource.loop = true;
        miniGameAudioSource.Play();
    }

    public void StopMinigameSong()
    {
        if (miniGameAudioSource.isPlaying)
        {
            miniGameAudioSource.Stop();
        }
    }

    void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(this);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(this);

        if (IsInitialized)
            return;

        miniGameAudioSource = gameObject.AddComponent<AudioSource>();

        mainSongAudioSource.loop = true;
        PlayMainSong();

        IsInitialized = true;
    }
}
