using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Board : MonoBehaviour
{
    //private Dice dice;
    private int numPlayers;
    //private int currentPlayerNum = 0;
    private GameObject[] playerArray;
    private const int numSpacesOnBoard = 40;
    private Transform[][] waypointsList;
    private Transform[] jailWaypoints;

    private Queue<Card> opportunityKnocksCards;
    private Queue<Card> potLuckCards;

    public GameObject waypointsHolder;

    private BoardTile[] boardTiles;
    public JSONReader reader;


    public Transform waypoint;
    public GameObject player;
    public Dice dice;

    // Start is called before the first frame update
    void Start()
    {
        // Reads data from file for properties and cards
        boardTiles = reader.GetBoardTiles();
        ShuffleCards();

        foreach (BoardTile boardTile in boardTiles)
        {
            boardTile.setBoard(this);
        }

        // Temporary hardcode of number of players
        numPlayers = 2;
        playerArray = new GameObject[numPlayers];

        CreateWaypoints();

        // Loops for each player, adding them to a list of all the players
        for (int i = 0; i < numPlayers; i++)
        {
            playerArray[i] = Instantiate(player, new Vector3(i - 24, 0, -24), Quaternion.identity);
            playerArray[i].GetComponent<Player>().setWaypoints(waypointsList[i]);
            playerArray[i].GetComponent<Player>().SetJailWaypoint(jailWaypoints[i]);
        }
    }

    public void SetOpportunityKnocksCards(Queue<Card> cards)
    {
        opportunityKnocksCards = cards;
    }

    public void SetPotluckCards(Queue<Card> cards)
    {
        potLuckCards = cards;
    }
    public Queue<Card> GetOpportunityKnocksCards()
    {
        return opportunityKnocksCards;
    }

    public Queue<Card> GetPotluckCards()
    {
        return potLuckCards;
    }

    private void ShuffleCards()
    {
        int randNum;

        Queue<Card> opportunityKnocksShuffled = new Queue<Card>();
        Queue<Card> potLuckShuffled = new Queue<Card>();

        List<Card> opportunityKnocksTemp;
        opportunityKnocksTemp = opportunityKnocksCards.ToList();

        List<Card> potLuckTemp;
        potLuckTemp = potLuckCards.ToList();

        while (opportunityKnocksTemp.Count > 0)
        {
            randNum = Random.Range(0, opportunityKnocksTemp.Count);
            opportunityKnocksShuffled.Enqueue(opportunityKnocksTemp[randNum]);
            //Debug.Log(opportunityKnocksTemp[randNum].description);
            opportunityKnocksTemp.RemoveAt(randNum);
        }

        while (potLuckTemp.Count > 0)
        {
            randNum = Random.Range(0, potLuckTemp.Count);
            potLuckShuffled.Enqueue(potLuckTemp[randNum]);
            //Debug.Log(potLuckTemp[randNum].description);
            potLuckTemp.RemoveAt(randNum);
        }

        opportunityKnocksCards = opportunityKnocksShuffled;
        potLuckCards = potLuckShuffled;
    }

    public GameObject[] GetPlayers()
    {
        return playerArray;
    }

    public int GetNumPlayers()
    {
        return numPlayers;
    }

    public BoardTile[] GetBoardTiles()
    {
        return boardTiles;
    }

    // Creates the waypoints for each player for where their piece goes on the board
    // Each player is given a list of waypoints unique to them
    private void CreateWaypoints()
    {
        waypointsList = new Transform[numPlayers][];
        jailWaypoints = new Transform[numPlayers];

        int x = -24;
        int z = -24;

        int gap = 4;
        int cornerGap = 8;
        int currentGap;
        int spaceNum = 0;
        int waypointX = 0;
        int waypointZ = 0;
        int spaceBetween = 2;

        for (int k = 0; k < numPlayers; k++)
        {
            waypointsList[k] = new Transform[numSpacesOnBoard];

            // Generates the waypoints for in jail
            if (k < 3)
            {
                waypointX = -21 + (k % 3);
                waypointZ = 22 - (k % 3);
            }
            else
            {
                waypointX = -22 + (k % 3);
                waypointZ = 21 - (k % 3);
            }

            jailWaypoints[k] = Instantiate(waypoint, new Vector3(waypointX, 0, waypointZ), Quaternion.identity);
        }

        // Loops for each side of the board
        for (int i = 0; i < 4; i++)
        {
            // Loops for each space on a single side of the board except the last
            for (int j = 0; j < 10; j++)
            {
                //Instantiate(waypoint, new Vector3(x, 0, z), Quaternion.identity);

                // Loops for each player in the game 
                for (int k = 0; k < numPlayers; k++)
                {
                    // Sets the waypoints X and Z coordinates for the jail space 
                    if (spaceNum == 10)
                    {
                        if (k < 3)
                        {
                            waypointX = x + (spaceBetween * (3 - k)) - 1;
                            waypointZ = z;
                        }
                        else
                        {
                            waypointX = x;
                            waypointZ = z + (spaceBetween * (2 - k)) + 1;
                        }
                    }
                    // Sets the waypoints X and Z coordinates for the other corner spaces
                    else if (spaceNum == 0 || spaceNum == 20 || spaceNum == 30)
                    {
                        switch (i)
                        {
                            case 0:
                                waypointX = x + (spaceBetween * (k % 3));
                                waypointZ = z + (spaceBetween * (k / 3));
                                break;
                            case 1:
                                waypointX = x + (spaceBetween * (k / 3));
                                waypointZ = z - (spaceBetween * (k % 3));
                                break;
                            case 2:
                                waypointX = x - (spaceBetween * (k % 3));
                                waypointZ = z - (spaceBetween * (k / 3));
                                break;
                            case 3:
                                waypointX = x - (spaceBetween * (k / 3));
                                waypointZ = z + (spaceBetween * (k % 3));
                                break;
                        }
                    }
                    // Sets the waypoints X and Z coordinates for every other space
                    else
                    {
                        switch (i)
                        {
                            case 0:
                                waypointX = x + (spaceBetween * (k % 3));
                                waypointZ = z + (spaceBetween * (k / 3)) - (spaceBetween / 2);
                                break;
                            case 1:
                                waypointX = x + (spaceBetween * (k / 3)) - (spaceBetween / 2);
                                waypointZ = z - (spaceBetween * (k % 3));
                                break;
                            case 2:
                                waypointX = x - (spaceBetween * (k % 3));
                                waypointZ = z - (spaceBetween * (k / 3)) + (spaceBetween / 2);
                                break;
                            case 3:
                                waypointX = x - (spaceBetween * (k / 3)) + (spaceBetween / 2);
                                waypointZ = z + (spaceBetween * (k % 3));
                                break;
                        }
                    }

                    // Creates a single waypoint at the coordinates specified
                    waypointsList[k][spaceNum] = Instantiate(waypoint, new Vector3(waypointX, 0, waypointZ), Quaternion.identity);
                    waypointsList[k][spaceNum].SetParent(waypointsHolder.transform);
                }

                // For the first and last space on a side (the corners) the gap between each space is set to the corner gap
                // otherwise it is the normal gap
                switch (j)
                {
                    case 0:
                    case 9:
                        currentGap = cornerGap;
                        break;
                    default:
                        currentGap = gap;
                        break;
                }

                // Depending on the side of the board, the Z or X coordinate is increased or decreased by the current gap
                switch (i)
                {
                    case 0:
                        z += currentGap;
                        break;
                    case 1:
                        x += currentGap;
                        break;
                    case 2:
                        z -= currentGap;
                        break;
                    case 3:
                        x -= currentGap;
                        break;
                }
                spaceNum++;
            }
        }
    }
}
