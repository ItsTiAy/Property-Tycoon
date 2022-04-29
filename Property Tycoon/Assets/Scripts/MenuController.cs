using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controls the objects for the menu
/// </summary>
public class MenuController : MonoBehaviour
{
    public GameObject playerOptions;
    public GameObject playerContainer;
    public TMP_Dropdown colour;
    public Button addPlayerButton;
    public Button startGameButton;
    public TMP_InputField timeLimit;

    private List<GameObject> players = new List<GameObject>();
    /// <summary>
    /// Adds new player to the start game menu
    /// </summary>
    public void AddPlayer()
    {
        GameObject player = Instantiate(playerOptions, playerContainer.transform, false);
        players.Add(player);
        player.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        player.GetComponentInChildren<Button>().onClick.AddListener(delegate 
        { 
            Destroy(player); 
            InitialData.NumPlayers -= 1; 
            addPlayerButton.interactable = true;
            if (InitialData.NumPlayers < 2)
            {
                startGameButton.interactable = false;
            }
            InitialData.PlayerColours[players.IndexOf(player)] = Color.red;
            players.Remove(player);
        });

        player.GetComponentsInChildren<TMP_Text>()[1].text = "Player " + (players.IndexOf(player) + 1);

        player.GetComponentInChildren<TMP_Dropdown>().onValueChanged.AddListener(delegate
        {
            InitialData.PlayerColours[players.IndexOf(player)] = GetColour(player.GetComponentInChildren<TMP_Dropdown>());
        });

        InitialData.NumPlayers += 1;
        Debug.Log("Num players " + InitialData.NumPlayers);
        Debug.Log("Index of player " + players.IndexOf(player));
        //Debug.Log("" InitialData.PlayerColours.Count);
        InitialData.PlayerColours[players.IndexOf(player)] = Color.red;

        if (InitialData.NumPlayers >= 2)
        {
            startGameButton.interactable = true;
        }

        if(InitialData.NumPlayers >= 6)
        {
            addPlayerButton.interactable = false;
        }
    }

    private void Update()
    {
        // Quits game if you press the escape key
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    public void SetAbridged(bool abridged)
    {
        InitialData.IsFullGame = !abridged;
        timeLimit.interactable = abridged;
        timeLimit.text = "";
    }

    private Color GetColour(TMP_Dropdown option)
    {
        return option.value switch
        {
            0 => Color.red,
            1 => Color.blue,
            2 => Color.yellow,
            3 => Color.cyan,
            4 => Color.black,
            5 => Color.green,
            _ => Color.red,
        };
    }    
}
