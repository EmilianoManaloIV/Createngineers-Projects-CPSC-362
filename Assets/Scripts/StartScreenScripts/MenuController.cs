using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuController : MonoBehaviour  
{ 
    // Buttons on "Start Screen" scene
    public void OnStartClick()
    {
        SoundManager.Instance.StartClickSound();
        // Change "StartGameScene" to the actual name of the game scene
        SceneManager.LoadScene("StartGameScene");
    }
    public void OnExitClick()
    {
        SoundManager.Instance.ExitClickSound();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    // Buttons on "Start Game" scene
    public void OnStartGameClick()
    {
        SoundManager.Instance.StartGameClickSound();
        SceneManager.LoadScene("MainGameScene"); 
    }
    public void OnBackClick()
    {
        SoundManager.Instance.BackClickSound();
        SceneManager.LoadScene("StartScreenScene");
    }

    public void OnQuitClick()
    {
        SoundManager.Instance.PlayMenuMusic();
        DOTween.KillAll();
        SceneManager.LoadScene("StartScreenScene");
    }

    public void OnPlayAgainClick()
    {
        SoundManager.Instance.PlayMenuMusic();
        DOTween.KillAll();
        SceneManager.LoadScene("StartGameScene");
    }

    public void OnButtonClicked()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlayClick();
    }
}