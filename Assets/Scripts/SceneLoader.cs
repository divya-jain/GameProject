using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles Loading Game Scene And Quit App functions
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public int _loadSceneAtIndex;

    public void OnPlayGame()
    {
        //Loads Game Scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(_loadSceneAtIndex);
    }
    
    public void OnQuitGame()
    {
        //quits the game
        Application.Quit();
    }
}