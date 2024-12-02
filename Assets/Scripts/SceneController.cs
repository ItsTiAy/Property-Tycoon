using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Controls the change of scenes
/// </summary>
public class SceneController : MonoBehaviour
{
    public TMP_InputField timeLimit;
    //public GameObject playerContainer;

    /// <summary>
    /// Loads the game board
    /// </summary>
    public void LoadGame()
    {
        if (!InitialData.IsFullGame)
        {
            InitialData.TimeLimit = int.Parse(timeLimit.text);
        }

        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Loads the menu
    /// </summary>
    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
}
