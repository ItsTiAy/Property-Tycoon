using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Controller : MonoBehaviour
{
    private int currentPlayerNum;
    //private int numSpacesOnBoard = 40;
    //private int newPlayerPosition;
    //private int currentPlayerPosition;
    //private bool isPlayerMoving;

    //private BoardTile[] boardTiles;
    //private GameObject[] playerArray;
    private Player currentPlayer;
    //private Transform[][] waypointsList;

    //public int numPlayers;

    //public GameObject player;
    public Dice dice;
    public Board board;
    //public JSONReader reader;
    //public Transform waypoint;
    //public GameObject propertyPopup;
    //public GameObject potLuckPopup;
    //public GameObject opportunityKnocksPopup;
    //public TMP_Text propertyName;
    //public TMP_Text rentPrices;
    ////public Button buyButton;
    //public Button auctionButton;
    public Button rollDiceButton;

    // Start is called before the first frame update
    void Start()
    {
        currentPlayer = board.GetPlayers()[currentPlayerNum].GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPlayer.IsMoving())
        {
            if(currentPlayer.MovePlayerPiece())
            {
                TileAction();
            }
        }
    }

    public void MovePlayerForward() //TODO: Figure out where all the different things are meant to go, which class has what in it
    {
        // Sets currentPlayer to the player in the list of players at position of currentPlayerNum 
        currentPlayer = board.GetPlayers()[currentPlayerNum].GetComponent<Player>();

        // Runs MovePlayer method for the current player
        //currentPlayer.UpdatePosition(10);
        currentPlayer.UpdatePosition(dice.RollDice());
        currentPlayer.SetMoving(true);
        rollDiceButton.interactable = false;

        // currentPlayerNum is increased by 1 and loops back to the first player after the last player has had their turn
        currentPlayerNum = (currentPlayerNum + 1) % board.GetNumPlayers();
    }

    private void TileAction()
    {
        BoardTile currentTile = board.GetBoardTiles()[currentPlayer.GetCurrentPosition()];

        currentTile.PerformAction(currentPlayer);
    }
}
