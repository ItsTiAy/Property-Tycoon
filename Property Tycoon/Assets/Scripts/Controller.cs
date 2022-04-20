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

    public GameObject propertySquare;
    public Button viewProperties;
    public GameObject viewPropertiesMenu;
    public GameObject propertySquareHolder;
    public GameObject propertyInfoPanel;
    public TMP_Text propertyInfoPanelName;
    public TMP_Text propertyInfoPanelRent;
    public Button buyHouse;
    public Button sellHouse;
    public Button mortgage;


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

        if (dice.WasDouble())
        {
            Debug.Log("Double");
            currentPlayer.IncreaseNumDoubles();
        }
        else
        {
            currentPlayerNum = (currentPlayerNum + 1) % board.GetNumPlayers();
            currentPlayer.ResetNumDoubles();
        }

        if(currentPlayer.GetNumDoubles() >= 3)
        {
            currentPlayer.GoToJail();
            currentPlayer.ResetNumDoubles();

            // currentPlayerNum is increased by 1 and loops back to the first player after the last player has had their turn
            currentPlayerNum = (currentPlayerNum + 1) % board.GetNumPlayers();
        }
        else
        {
            currentPlayer.SetMoving(true);
            rollDiceButton.interactable = false;
        }
    }

    private void TileAction()
    {
        BoardTile currentTile = board.GetBoardTiles()[currentPlayer.GetCurrentPosition()];

        currentTile.PerformAction(currentPlayer);
    }

    public void ShowProperties()
    {
        viewProperties.GetComponent<Button>().onClick.RemoveAllListeners();
        if (viewProperties.GetComponentInChildren<TMP_Text>().text == "View Properties")
        {
            viewPropertiesMenu.SetActive(true);

            viewProperties.GetComponentInChildren<TMP_Text>().text = "Back";
            viewProperties.onClick.AddListener(delegate
            {
                propertyInfoPanel.SetActive(false);
                viewPropertiesMenu.SetActive(false);

                foreach (Transform child in propertySquareHolder.transform)
                {
                    Destroy(child.gameObject);
                }
            });

            List<BuyableTile> sortedProperties = currentPlayer.properties;
            List<Station> stations = new List<Station>();
            List<Utility> utilities = new List<Utility>();

            sortedProperties.Sort((x, y) => x.position.CompareTo(y.position));
            foreach (BuyableTile tile in sortedProperties)
            {
                if (tile.GetType() == typeof(Station))
                {
                    stations.Add((Station)tile);
                }
                else if (tile.GetType() == typeof(Utility))
                {
                    utilities.Add((Utility)tile);
                }
                else
                {
                    Property property = (Property)tile;
                    GameObject square = Instantiate(propertySquare);
                    square.GetComponent<Image>().color = property.GetGroupColour();

                    square.GetComponent<Button>().onClick.RemoveAllListeners();
                    square.GetComponent<Button>().onClick.AddListener(delegate
                    {
                        // Removes any listeners previously added to avoid adding multiple to one button
                        mortgage.onClick.RemoveAllListeners();
                        buyHouse.onClick.RemoveAllListeners();
                        sellHouse.onClick.RemoveAllListeners();

                        propertyInfoPanelName.text = property.tileName;

                        propertyInfoPanelRent.text = "";
                        for (int i = 0; i < 6; i++)
                        {
                            propertyInfoPanelRent.text += (property.rent[i] + "\n");
                        }

                        if (property.IsMortgaged())
                        {
                            mortgage.GetComponentInChildren<TMP_Text>().text = "Unmortgage";
                        }
                        else
                        {
                            mortgage.GetComponentInChildren<TMP_Text>().text = "Mortgage";
                        }

                        mortgage.onClick.AddListener(delegate
                        {
                            if (property.IsMortgaged())
                            {
                                property.SetMortgaged(false);
                                mortgage.GetComponentInChildren<TMP_Text>().text = "Mortgage";

                                if (currentPlayer.GetSameGroupProperties(property).Count == property.groupAmount)
                                {
                                    buyHouse.interactable = true;
                                }
                            }
                            else
                            {
                                property.SetMortgaged(true);
                                mortgage.GetComponentInChildren<TMP_Text>().text = "Unmortgage";

                                buyHouse.interactable = false;
                            }
                        });

                        buyHouse.onClick.AddListener(delegate
                        {
                            currentPlayer.BuyHouse(property);
                            sellHouse.interactable = true;
                            mortgage.interactable = false;

                            if (property.GetNumHouses() >= 5)
                            {
                                buyHouse.interactable = false;
                            }

                            if (currentPlayer.GetMoney() < property.GetHousePrice())
                            {
                                buyHouse.interactable = false;
                            }
                        });

                        sellHouse.onClick.AddListener(delegate
                        {
                            currentPlayer.SellHouse(property);
                            buyHouse.interactable = true;

                            if (property.GetNumHouses() <= 0)
                            {
                                sellHouse.interactable = false;
                            }

                            if (property.GetNumHouses() <= 0)
                            {
                                mortgage.interactable = true;
                            }
                        });

                        if (currentPlayer.GetSameGroupProperties(property).Count != property.groupAmount
                            || currentPlayer.GetMoney() < property.GetHousePrice()
                            || property.IsMortgaged()
                            || property.GetNumHouses() >= 5)
                        {
                            buyHouse.interactable = false;
                        }
                        else
                        {
                            buyHouse.interactable = true;
                        }

                        if (property.GetNumHouses() <= 0)
                        {
                            sellHouse.interactable = false;
                        }
                        else
                        {
                            sellHouse.interactable = true;
                        }

                        if (property.GetNumHouses() > 0)
                        {
                            mortgage.interactable = false;
                        }
                        else
                        {
                            mortgage.interactable = true;
                        }

                        buyHouse.gameObject.SetActive(true);
                        sellHouse.gameObject.SetActive(true);
                        propertyInfoPanel.SetActive(true);
                    });
                    square.transform.SetParent(propertySquareHolder.transform, false);
                }
            }

            foreach (Station station in stations)
            {
                GameObject square = Instantiate(propertySquare);
                square.GetComponent<Image>().color = station.GetGroupColour();
                square.GetComponent<Button>().onClick.AddListener(delegate
                {
                    mortgage.onClick.RemoveAllListeners();

                    propertyInfoPanelName.text = station.tileName;

                    propertyInfoPanelRent.text = "";
                    for (int i = 0; i < 4; i++)
                    {
                        propertyInfoPanelRent.text += (station.rent[i] + "\n");
                    }

                    mortgage.onClick.AddListener(delegate
                    {
                        if (station.IsMortgaged())
                        {
                            station.SetMortgaged(false);
                            mortgage.GetComponentInChildren<TMP_Text>().text = "Mortgage";
                        }
                        else
                        {
                            station.SetMortgaged(true);
                            mortgage.GetComponentInChildren<TMP_Text>().text = "Unmortgage";
                        }
                    });

                    if (station.IsMortgaged())
                    {
                        mortgage.GetComponentInChildren<TMP_Text>().text = "Unmortgage";
                    }
                    else
                    {
                        mortgage.GetComponentInChildren<TMP_Text>().text = "Mortgage";
                    }

                    buyHouse.gameObject.SetActive(false);
                    sellHouse.gameObject.SetActive(false);
                    propertyInfoPanel.SetActive(true);
                });
                square.transform.SetParent(propertySquareHolder.transform, false);
            }
            foreach (Utility utility in utilities)
            {
                GameObject square = Instantiate(propertySquare);
                square.GetComponent<Image>().color = utility.GetGroupColour();
                square.GetComponent<Button>().onClick.AddListener(delegate
                {
                    mortgage.onClick.RemoveAllListeners();

                    propertyInfoPanelName.text = utility.tileName;

                    propertyInfoPanelRent.text = "";
                    for (int i = 0; i < 2; i++)
                    {
                        propertyInfoPanelRent.text += (utility.rent[i] + "\n");
                    }

                    mortgage.onClick.AddListener(delegate
                    {
                        if (utility.IsMortgaged())
                        {
                            utility.SetMortgaged(false);
                            mortgage.GetComponentInChildren<TMP_Text>().text = "Mortgage";
                        }
                        else
                        {
                            utility.SetMortgaged(true);
                            mortgage.GetComponentInChildren<TMP_Text>().text = "Unmortgage";
                        }
                    });

                    if (utility.IsMortgaged())
                    {
                        mortgage.GetComponentInChildren<TMP_Text>().text = "Unmortgage";
                    }
                    else
                    {
                        mortgage.GetComponentInChildren<TMP_Text>().text = "Mortgage";
                    }

                    buyHouse.gameObject.SetActive(false);
                    sellHouse.gameObject.SetActive(false);
                    propertyInfoPanel.SetActive(true);
                });
                square.transform.SetParent(propertySquareHolder.transform, false);
            }
        }
        else
        {
            viewProperties.GetComponentInChildren<TMP_Text>().text = "View Properties";

            viewProperties.onClick.AddListener(delegate
            {
                viewPropertiesMenu.SetActive(true);
            });
        }
    }       
}
