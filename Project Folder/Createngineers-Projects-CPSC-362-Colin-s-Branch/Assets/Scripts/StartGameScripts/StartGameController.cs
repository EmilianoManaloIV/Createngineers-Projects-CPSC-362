using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameController : MonoBehaviour
{
    public void OnStartClick()
    {
        // Change "MainGameScene" to the actual name of the game scene
        SceneManager.LoadScene("MainGameScene");
    }
    public void OnBackClick()
    {
        SceneManager.LoadScene("StartScreenScene");
    }
}
