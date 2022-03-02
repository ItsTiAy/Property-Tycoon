using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private Dice dice;
    private int numPlayers;
    private int currentPlayerNum = 0;
    private GameObject[] playerList;
    private Player currentPlayer;
    private const int numSpacesOnBoard = 40;
    private Transform[][] waypointsList;
    private Transform[] waypoints;

    public Transform waypoint;
    public GameObject player;
    public GameObject theDice;

    // Start is called before the first frame update
    void Start()
    {
        // Creates new dice object 
        dice = Instantiate(theDice).GetComponent<Dice>();
        // Temporary hardcode of number of players
        numPlayers = 6;
        playerList = new GameObject[numPlayers];

        CreateWaypoints();

        // Loops for each player, adding them to a list of all the players
        for (int i = 0; i < numPlayers; i++)
        {
            playerList[i] = Instantiate(player, new Vector3(i - 24, 0, -24), Quaternion.identity);
            playerList[i].GetComponent<Player>().setWaypoints(waypointsList[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeTurn()
    {
        // Sets currentPlayer to the player in the list of players at position of currentPlayerNum
        currentPlayer = playerList[currentPlayerNum].GetComponent<Player>();

        // Runs MovePlayer method for the current player
        currentPlayer.MovePlayer(dice.RollDice());

        // currentPlayerNum is increased by 1 and loops back to the first player after the last player has had their turn
        currentPlayerNum = (currentPlayerNum + 1) % numPlayers;
    }

    // Creates the waypoints for each player for where their piece goes on the board
    // Each player is given a list of waypoints unique to them
    private void CreateWaypoints()
    {
        waypointsList = new Transform[numPlayers][];
        //waypoints = new Transform[numSpacesOnBoard];

        int x = -24;
        int z = -24;

        for (int k = 0; k < numPlayers; k++)
        {
            waypointsList[k] = new Transform[numSpacesOnBoard];
        }

        /*
        for (int i = 0; i < numSpacesOnBoard; i++)
        {
            for (int j = 0; j < numPlayers; j++)
            {
                waypointsList[j][i] = Instantiate(waypoint, new Vector3(x + x2, 0, z + z2), Quaternion.identity);

                if (i < 10)
                {
                    x2 += 1;
                }
                else if (i >= 10 && i < 20)
                {
                    z2 -= 1;
                }
                else if (i >= 20 && i < 30)
                {
                    x2 -= 1;
                }
                else
                {
                    z2 += 1;
                }
            }

            x2 = 0;
            z2 = 0;

            if (i < 10)
            {
                z += 4;
            }
            else if (i >= 10 && i < 20)
            {
                x += 4;
            }
            else if (i >= 20 && i < 30)
            {
                z -= 4;
            }
            else
            {
                x -= 4;
            }
        }
*/
        int gap = 4;
        int cornerGap = 8;
        int currentGap;
        int spaceNum = 0;
        int waypointX = 0;
        int waypointZ = 0;
        int spaceBetween = 2;

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
                        if(k < 3)
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
