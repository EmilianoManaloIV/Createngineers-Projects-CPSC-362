using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// This is the "immortal" object that carries the bot count
/// from the selection scene to the main game scene.

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;

    public int botCount = 3; // Default to 3

    [Header("Scene UI Links")]
    [Tooltip("Drag your '1 Bot' button here")]
    public Button bot1Button;
    [Tooltip("Drag your '2 Bots' button here")]
    public Button bot2Button;
    [Tooltip("Drag your '3 Bots' button here")]
    public Button bot3Button;
    [Tooltip("Drag your 'Start Game' button here")]
    public Button startButton;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            
            // Link the buttons that we have in the Inspector
            LinkButtons();
        }
        else
        {   
            // 1. Remove the broken listeners from the scene's buttons
            this.bot1Button.onClick.RemoveAllListeners();
            this.bot2Button.onClick.RemoveAllListeners();
            this.bot3Button.onClick.RemoveAllListeners();
            this.startButton.onClick.RemoveAllListeners();

            // 2. Add new listeners pointing to the IMMORTAL 'Instance'
            this.bot1Button.onClick.AddListener(Instance.SetBotCount_1);
            this.bot2Button.onClick.AddListener(Instance.SetBotCount_2);
            this.bot3Button.onClick.AddListener(Instance.SetBotCount_3);
            this.startButton.onClick.AddListener(Instance.StartMainGame);

            // 3. Now that the buttons are fixed, destroy this duplicate object
            Destroy(this.gameObject);
        }
    }

    // A helper function to link the buttons on this component.
    private void LinkButtons()
    {
        // We check if the buttons are assigned, just to be safe!
        if (bot1Button != null) bot1Button.onClick.AddListener(SetBotCount_1);
        if (bot2Button != null) bot2Button.onClick.AddListener(SetBotCount_2);
        if (bot3Button != null) bot3Button.onClick.AddListener(SetBotCount_3);
        if (startButton != null) startButton.onClick.AddListener(StartMainGame);
    }

    // --- Public Button Functions ---

    public void SetBotCount_1()
    {
        botCount = 1;
        if (SoundManager.Instance != null) SoundManager.Instance.PlayClick();
        Debug.Log("Bot count set to: 1");
    }
    
    public void SetBotCount_2()
    {
        botCount = 2;
        if (SoundManager.Instance != null) SoundManager.Instance.PlayClick();
        Debug.Log("Bot count set to: 2");
    }
    
    public void SetBotCount_3()
    {
        botCount = 3;
        if (SoundManager.Instance != null) SoundManager.Instance.PlayClick();
        Debug.Log("Bot count set to: 3");
    }

    public void StartMainGame()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlayClick();
        // Make sure "MainGameScene" is in your File > Build Settings!
        SceneManager.LoadScene("MainGameScene"); 
    }
}