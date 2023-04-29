using UnityEngine;
using UnityEngine.SceneManagement;



public class SongManager : MonoBehaviour
{
    public static SongManager Instance { get; private set; }

    [field:SerializeField] public bool IsInitialized { get; private set; }

    public AudioSource mainSongAudioSource;

    void Start()
    {
        // mainSongAudioSource = GetComponent<AudioSource>();
    }

    public void PlayMainSong()
    {
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

        IsInitialized = true;

        PlayMainSong();
    }
}
