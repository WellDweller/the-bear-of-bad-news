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
    private HashSet<int> playedMinigameSongIndices;

    public AudioClip typingAudioClip;
    private AudioSource typingAudioSource;

    public AudioClip footstepsAudioClip;
    private AudioSource footstepsAudioSource;

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
            PlayMinigameSong();
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

        // Keep alive across scenes
        DontDestroyOnLoad(this);

        if (IsInitialized)
            return;

        // Minigame Songs
        miniGameAudioSource = gameObject.AddComponent<AudioSource>();
        playedMinigameSongIndices = new HashSet<int>();

        // Typing Sounds
        typingAudioSource = gameObject.AddComponent<AudioSource>();

        // Footstep Sounds
        footstepsAudioSource = gameObject.AddComponent<AudioSource>();

        // Main Song
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
                LoadAudioClip("SFX/big_negative/big_negative_1", volume: 0.7f),
                LoadAudioClip("SFX/big_negative/big_negative_2", volume: 0.3f),
            },
            new List<AudioSource> () {
                LoadAudioClip("SFX/small_negative/small_negative_1", volume: 0.5f),
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

        // If all songs have been played, clear the set of played song indices
        if (playedMinigameSongIndices.Count == miniGameAudioClips.Count)
        {
            Debug.Log("All minigame songs have been played, resetting.");
            playedMinigameSongIndices.Clear();
        }

        int songIndex;
        do
        {
            // Pick a random song index
            songIndex = Random.Range(0, miniGameAudioClips.Count);
        }
        // Keep generating random song indices until an unplayed one is found
        while (playedMinigameSongIndices.Contains(songIndex));

        // Add the chosen song index to the played set
        playedMinigameSongIndices.Add(songIndex);

        // Play the song
        miniGameAudioSource.clip = miniGameAudioClips[songIndex];
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

    //
    // Typing
    //

    public void PlayTyping()
    {
        Debug.Log("Play typing!");
        typingAudioSource.clip = typingAudioClip;
        typingAudioSource.loop = true;
        typingAudioSource.Play();
    }

    public void PauseTyping()
    {
        Debug.Log("Pause typing!");
        typingAudioSource.Pause();
    }

    //
    // Footsteps
    //

    public void PlayFootsteps()
    {
        footstepsAudioSource.clip = footstepsAudioClip;
        footstepsAudioSource.loop = true;
        footstepsAudioSource.Play();
    }

    public void PauseFootsteps()
    {
        footstepsAudioSource.Pause();
    }
}
