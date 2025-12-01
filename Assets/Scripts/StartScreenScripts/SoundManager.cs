using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is a persistent Singleton (immortal object) that manages
/// all audio in the game. Place it in your FIRST scene.
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    // We need two audio sources: one for looping music,
    // and one for one-shot sound effects.
    private AudioSource musicSource;
    private AudioSource sfxSource;

    [Header("Music Clips")]
    public AudioClip menuMusic;
    public AudioClip gameMusic; 

    [Header("Sound Effect Clips")]
    public AudioClip gameStartShuffle;
    public AudioClip drawCard;
    public AudioClip playCard;
    public AudioClip drawTwo;
    public AudioClip drawFour;
    public AudioClip skipTurn;
    public AudioClip reverseTurn;
    public AudioClip callUno;
    public AudioClip winGame;
    public AudioClip buttonClick;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Create our AudioSource components
            musicSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();

            // Configure them
            musicSource.loop = true;
        }
        else
        {
            // If one already exists, destroy this new one
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Start the menu music as soon as the game loads
        if (menuMusic != null)
        {
            musicSource.clip = menuMusic;
            musicSource.Play();
        }
    }

    // A private helper function to safely play a sound.
    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // Called by GameManager when the game starts.
    public void PlayGameStart()
    {
        musicSource.Stop(); // Stop the menu music
        PlaySound(gameStartShuffle);
        
        if (gameMusic != null) 
        {
            musicSource.clip = gameMusic;
            musicSource.Play();
        }
    }

    public void PlayMenuMusic()
    {
        // Check if menu music is already playing
        if (musicSource.clip == menuMusic && musicSource.isPlaying)
        {
            return; // Don't restart if it's already playing
        }
        
        musicSource.Stop();
        if (menuMusic != null)
        {
            musicSource.clip = menuMusic;
            musicSource.Play();
        }
    }

    // Buttons on "Start Screen" scene
    public void StartClickSound()
    {
        if (Instance != null) Instance.PlayClick();
    }
    public void ExitClickSound()
    {
        if (Instance != null) Instance.PlayClick();
    }

    // Buttons on "Start Game" scene
    public void StartGameClickSound()
    {
        if (Instance != null) Instance.PlayClick();
    }
    public void BackClickSound()
    {
        if (Instance != null) Instance.PlayClick();
    }

    // Sound effect triggers for game events
    public void PlayDrawCard() => PlaySound(drawCard);
    public void PlayPlayCard() => PlaySound(playCard);
    public void PlayDrawTwo() => PlaySound(drawTwo);
    public void PlayDrawFour() => PlaySound(drawFour);
    public void PlaySkip() => PlaySound(skipTurn);
    public void PlayReverse() => PlaySound(reverseTurn);
    public void PlayUno() => PlaySound(callUno);
    public void PlayWin() => PlaySound(winGame);
    public void PlayClick() => PlaySound(buttonClick);
}