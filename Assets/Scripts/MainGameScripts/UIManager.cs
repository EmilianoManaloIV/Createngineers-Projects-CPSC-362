using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

// Manages all UI elements in the MainGameScene.

public class UIManager : MonoBehaviour
{
    [Header("Game Manager")]
    
    [Header("Status")]
    public TextMeshProUGUI statusText;

    [Header("Buttons")]
    public Button drawPileButton;
    public Button unoButton;
    public Button passTurnButton;
    public Button pauseButton;
    
    [Header("Panels")]
    public GameObject colorPickerPanel;
    public GameObject winnerPanel;
    public TextMeshProUGUI winnerText;
    public GameObject pausePanel;
    
    [Header("Panel Buttons")]
    public List<Button> colorPickerButtons; // 0=Red, 1=Green, 2=Blue, 3=Yellow
    public Button resumeButton;

    [Header("Rules Menu")]
    public Button rulesButton;
    public GameObject rulesPanel;
    public Button rulesPanelBackButton;

    [Header("Indicators")]
    public Image discardPileImage;
    public Image wildColorIndicator;

    void Start()
    {
        // Add listeners to buttons
        drawPileButton.onClick.AddListener(OnDrawClicked);
        unoButton.onClick.AddListener(OnUnoClicked);
        passTurnButton.onClick.AddListener(OnPassClicked);
        pauseButton.onClick.AddListener(OnPauseClicked);
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(OnResumeClicked);
        }

        if (rulesButton != null)
        {
            rulesButton.onClick.AddListener(OnRulesClicked);
        }
        if (rulesPanelBackButton != null)
        {
            rulesPanelBackButton.onClick.AddListener(OnRulesBackClicked);
        }

        // Add listeners to color picker buttons
        colorPickerPanel.transform.Find("RedButton").GetComponent<Button>().onClick.AddListener(() => OnColorSelected(CardColor.Red));
        colorPickerPanel.transform.Find("YellowButton").GetComponent<Button>().onClick.AddListener(() => OnColorSelected(CardColor.Yellow));
        colorPickerPanel.transform.Find("GreenButton").GetComponent<Button>().onClick.AddListener(() => OnColorSelected(CardColor.Green));
        colorPickerPanel.transform.Find("BlueButton").GetComponent<Button>().onClick.AddListener(() => OnColorSelected(CardColor.Blue));

        
        // Hide panels at the start
        colorPickerPanel.SetActive(false);
        winnerPanel.SetActive(false);
        wildColorIndicator.gameObject.SetActive(false);
        pausePanel.SetActive(false);
        
        passTurnButton.gameObject.SetActive(false); // Hide at start
        unoButton.gameObject.SetActive(false); // Hide at start
    }

    // --- Public Methods to Control UI ---
    
    public void UpdateStatusText(string message)
    {
        statusText.text = message;
    }
    
    public void UpdateDiscardPile(CardData topCard)
    {
        discardPileImage.sprite = topCard.cardImage;
    }

    public void ShowDrawButton(bool show)
    {
        drawPileButton.gameObject.SetActive(show);
    }
    
    public void ShowPassButton(bool show)
    {
        passTurnButton.gameObject.SetActive(show);
    }
    
    public void ShowUnoButton(bool show)
    {
        if (unoButton != null)
        {
            unoButton.gameObject.SetActive(show);
        }
    }

    public void ShowColorPicker(bool show)
    {
        colorPickerPanel.SetActive(show);
    }

    public void UpdateWildColorIndicator(CardColor? color)
    {
        if (color == null)
        {
            // If the color is null, HIDE the indicator
            wildColorIndicator.gameObject.SetActive(false);
        }
        else
        {
            wildColorIndicator.gameObject.SetActive(true);
            
            switch (color)
            {
                case CardColor.Red: wildColorIndicator.color = Color.red; break;
                case CardColor.Green: wildColorIndicator.color = Color.green; break;
                case CardColor.Blue: wildColorIndicator.color = Color.blue; break;
                case CardColor.Yellow: wildColorIndicator.color = Color.yellow; break;
            }
        }
    }

    public void ShowPausePanel(bool show)
    {
        pausePanel.SetActive(show);
    }
    
    public void ShowWinnerScreen(string winnerMessage)
    {
        winnerText.text = winnerMessage;
        winnerPanel.SetActive(true);
    }

    // --- Button Listeners ---
    private void OnDrawClicked()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlayDrawCard();
        GameManager.Instance.OnDrawPileClicked();
    }

    private void OnUnoClicked()
    {
        GameManager.Instance.OnUnoButtonClicked();
    }
    
    private void OnPassClicked()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlayClick();
        GameManager.Instance.OnPassTurnClicked();
    }
    private void OnPauseClicked()
    {
        GameManager.Instance.TogglePauseGame();
    }
    private void OnResumeClicked()
    {
        GameManager.Instance.TogglePauseGame();
    }

    private void OnRulesClicked()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlayClick();
        
        // Hide the pause panel
        pausePanel.SetActive(false);
        // Show the rules panel
        rulesPanel.SetActive(true);
        
        // Tell the GameManager to HIDE all 3D cards
        GameManager.Instance.SetAllHandsVisibility(false);
    }
    
    private void OnRulesBackClicked()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlayClick();
        
        // Show the pause panel again
        pausePanel.SetActive(true);
        // Hide the rules panel
        rulesPanel.SetActive(false);
        
        // Tell the GameManager to SHOW all 3D cards
        GameManager.Instance.SetAllHandsVisibility(true);
    }
    
    private void OnColorSelected(CardColor color)
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlayClick();
        GameManager.Instance.SetWildColor(color);
    }

    public void OnButtonClicked()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlayClick();
    }
}