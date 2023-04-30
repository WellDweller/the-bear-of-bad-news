using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;



public class SongManager : MonoBehaviour
{
    public static SongManager Instance { get; private set; }

    [field: SerializeField] public bool IsInitialized { get; private set; }

    public AudioSource mainSongAudioSource;

    [SerializeField] public List<AudioClip> miniGameAudioClips;
    private AudioSource miniGameAudioSource;

    void Start()
    {
    }

    private Dictionary<string, List<List<AudioSource>>> SFXmap;

    //
    // Initialization
    //

    private AudioSource LoadAudioClip(string assetPath, float volume = 1.0f)
    {
        AudioClip audioClip = Resources.Load<AudioClip>(assetPath);
        if (audioClip == null)
        {
            Debug.LogError("Failed to load AudioClip at path: " + assetPath);
        }
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        return audioSource;

    }

    private void Update()
    {
        if (InputSystem.InputActions.Debug.DebugSound.WasPerformedThisFrame())
        {
            PlaySFX("negative");
        }

    }

    void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found existing SongManager, goodbye");
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(this);

        if (IsInitialized)
            return;

        miniGameAudioSource = gameObject.AddComponent<AudioSource>();

        mainSongAudioSource.loop = true;
        PlayMainSong();

        // Setup SFX
        var sfx_click = new List<List<AudioSource>>()
        {
            new List<AudioSource> () {
                LoadAudioClip("SFX/click/click_1"),
                LoadAudioClip("SFX/click/click_3"),
                LoadAudioClip("SFX/click/click_4"),
            },
        };
        var sfx_success = new List<List<AudioSource>>()
        {
            new List<AudioSource> () {
                LoadAudioClip("SFX/success/success_2"),
            },
            new List<AudioSource> () {
                LoadAudioClip("SFX/pop/pop_1"),
                LoadAudioClip("SFX/pop/pop_2"),
                LoadAudioClip("SFX/pop/pop_3"),
                LoadAudioClip("SFX/pop/pop_4"),
            },
        };
        var sfx_medium = new List<List<AudioSource>>()
        {
            new List<AudioSource> () {
                LoadAudioClip("SFX/success/success_3"),
            },
            new List<AudioSource> () {
                LoadAudioClip("SFX/pop/pop_1"),
                LoadAudioClip("SFX/pop/pop_2"),
                LoadAudioClip("SFX/pop/pop_3"),
                LoadAudioClip("SFX/pop/pop_4"),
            },
        };
        var sfx_negative = new List<List<AudioSource>>()
        {
            new List<AudioSource> () {
                LoadAudioClip("SFX/big_negative/big_negative_1", 0.3f),
                LoadAudioClip("SFX/big_negative/big_negative_2", volume: 0.2f),
            },
            new List<AudioSource> () {
                LoadAudioClip("SFX/small_negative/small_negative_1", volume: 0.1f),
            },
            new List<AudioSource> () {
                LoadAudioClip("SFX/breaking/breaking_celery"),
                LoadAudioClip("SFX/breaking/breaking_glass"),
            },
        };
        var sfx_hover = new List<List<AudioSource>>()
        {
            new List<AudioSource> () {
                LoadAudioClip("SFX/hover/hover_1"),
            },
        };

        SFXmap = new Dictionary<string, List<List<AudioSource>>>()
        {
            {"click", sfx_click},
            {"success", sfx_success},
            {"medium", sfx_medium},
            {"negative", sfx_negative},
            {"hover", sfx_hover},
        };

        IsInitialized = true;
    }

    void OnDestroy()
    {
        Debug.Log("SongManager destroyed");
    }

    //
    // SFX
    //

    public void PlaySFX(string sfx_name)
    {
        List<List<AudioSource>> sfx;
        if (!SFXmap.TryGetValue(sfx_name, out sfx))
        {
            Debug.LogError("Couldn't find SFX " + sfx_name);
            return;
        }
        foreach (var sound_layer in sfx)
        {
            int randomIndex = Random.Range(0, sound_layer.Count);
            AudioSource randomAudioSource = sound_layer[randomIndex];
            randomAudioSource.Play();
        }
    }

    //
    // Main Song
    //

    public void PlayMainSong()
    {
        StopMinigameSong();

        if (!mainSongAudioSource.isPlaying)
        {
            Debug.Log("Playing main song");
            mainSongAudioSource.Play();
        }
    }

    public void PauseMainSong()
    {
        if (mainSongAudioSource.isPlaying)
        {
            Debug.Log("Pausing main song");
            mainSongAudioSource.Pause();
        }
    }

    //
    // Minigame Song
    //

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
}
