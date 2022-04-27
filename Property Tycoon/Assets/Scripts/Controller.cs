using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
/// <summary>
/// Controller class
/// </summary>
public class Controller : MonoBehaviour
{
    private int currentPlayerNum;
    private Player currentPlayer;

    public Dice dice;
    public Board board;

    public GameObject propertySquare;
    public Button viewProperties;
    public GameObject viewPropertiesMenu;
    public GameObject propertySquareHolder;
    public GameObject propertyInfoPanel;
    public GameObject GameEndPopup;

    //public GameObject propertyPopup;
    //public GameObject potLuckPopup;
    //public GameObject opportunityKnocksPopup;
    //public GameObject otherPropertyPopup;
    public GameObject taxPopup;

    public TMP_Text propertyInfoPanelName;
    public TMP_Text propertyInfoPanelRent;
    public TMP_Text dice1UI;
    public TMP_Text dice2UI;
    public Button buyHouse;
    public Button sellHouse;
    public Button mortgage;
    public Button sellProperty;
    public Button propertyPayPlayerButton;
    public Button rollDiceButton;
    public Button payOutOfJailButton;
    public Button useCardButton;
    public Button opportunityKnocksOkButton;
    public Button potLuckOkButton;
    public Button taxButton;
    public Button buyProperty;

    // Start is called before the first frame update
    void Start()
    {
        currentPlayer = board.GetPlayers()[currentPlayerNum].GetComponent<Player>();
        rollDiceButton.onClick.AddListener(MovePlayerForward);
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if player is moving
        if (currentPlayer.IsMoving())
        {
            // Moves the player pieces and checks if it has fininshed moving
            if(currentPlayer.MovePlayerPiece())
            {
                // Calls TileAction once the player has stopped moving
                TileAction();
            }
        }
    }

    /// <summary>
    /// Sets all values for the next players turn
    /// </summary>
    public void EndTurn()
    {
        Debug.Log(currentPlayer.GetCurrentPosition());
        // Updates the current player number
        currentPlayerNum = (currentPlayerNum + 1) % board.GetNumPlayers();
        // Sets currentPlayer to the player in the list of players at position of currentPlayerNum 
        currentPlayer = board.GetPlayers()[currentPlayerNum].GetComponent<Player>();

        // Ends the game if there is only one player left
        if (board.GetPlayers().Count <= 1)
        {
            EndGame();
        }

        // If the player is in jail
        if (currentPlayer.InJail())
        {
            // If the player has not already had two turns in jail
            if (currentPlayer.GetTurnsInJail() < 2)
            {
                currentPlayer.IncreaseTurnsInJail();

                payOutOfJailButton.gameObject.SetActive(true);
                useCardButton.gameObject.SetActive(true);
                //viewProperties.interactable = false;

                payOutOfJailButton.onClick.RemoveAllListeners();
                // Button for paying to get out of jail
                payOutOfJailButton.onClick.AddListener(delegate
                {
                    FreeParking freeparking = (FreeParking)board.GetBoardTiles()[20];
                    freeparking.AddFineMoney(50);
                    currentPlayer.DecreaseMoney(50);
                    currentPlayer.GetOutOfJail();
                    payOutOfJailButton.interactable = false;
                });

                useCardButton.onClick.RemoveAllListeners();

                if (currentPlayer.HasGetOutOfJailFreePotLuck())
                {
                    // Button for using card to get out of jail
                    useCardButton.onClick.AddListener(delegate
                    {
                        // Adds card back to respective queue
                        board.GetPotluckCards().Enqueue(currentPlayer.GetGetOutOfJailFreePotLuck());
                        currentPlayer.setGetOutOfJailFreePotluck(null);
                        currentPlayer.GetOutOfJail();
                        useCardButton.interactable = false;
                    });
                    useCardButton.interactable = true;
                }
                else if (currentPlayer.HasGetOutOfJailFreeOpportunityKnocks())
                {
                    // Button for using card to get out of jail
                    useCardButton.onClick.AddListener(delegate
                    {
                        // Adds card back to respective queue
                        board.GetOpportunityKnocksCards().Enqueue(currentPlayer.GetGetOutOfJailFreeOpportunityKnocks());
                        currentPlayer.setGetOutOfJailFreeOpportunityKnocks(null);
                        currentPlayer.GetOutOfJail();
                        useCardButton.interactable = false;
                    });
                    useCardButton.interactable = true;
                }
                // If the player doesn't have a get out of jail free card
                else
                {
                    useCardButton.interactable = false;
                }

                // Pay to get out of jail button not interactable if the player has less than £50
                payOutOfJailButton.interactable = currentPlayer.GetMoney() >= 50;
            }
            // If the player is on their third turn in jail
            else
            {
                currentPlayer.GetOutOfJail();
                currentPlayer.ResetTurnsInJail();
            }
        }
        // If the player is not in jail
        else
        {
            rollDiceButton.GetComponentInChildren<TMP_Text>().text = "Roll Dice";
            rollDiceButton.onClick.RemoveAllListeners();

            // Sets roll dice button to move the player forward
            rollDiceButton.onClick.AddListener(MovePlayerForward);

            payOutOfJailButton.gameObject.SetActive(false);
            useCardButton.gameObject.SetActive(false);
            viewProperties.interactable = true;
        }
    }

    /// <summary>
    /// Rolls the dice and moves the player
    /// </summary>
    public void MovePlayerForward()
    {

        // Runs MovePlayer method for the current player
        //currentPlayer.UpdatePosition(30);
        currentPlayer.UpdatePosition(dice.RollDice());

        dice1UI.text = dice.GetDice1().ToString();
        dice2UI.text = dice.GetDice2().ToString();

        currentPlayer.SetDiceRollValue(dice.GetDice1() + dice.GetDice2());

        if (dice.WasDouble())
        {
            Debug.Log("Double");
            currentPlayer.IncreaseNumDoubles();
        }
        else
        {
            // If not a double
            currentPlayer.ResetNumDoubles();
            rollDiceButton.GetComponentInChildren<TMP_Text>().text = "End Turn";
            rollDiceButton.onClick.RemoveAllListeners();
            rollDiceButton.onClick.AddListener(EndTurn);
        }

        // If the player has got three doubles in a row
        if (currentPlayer.GetNumDoubles() >= 3)
        {
            currentPlayer.GoToJail();
            currentPlayer.ResetNumDoubles();
            rollDiceButton.GetComponentInChildren<TMP_Text>().text = "End Turn";
            rollDiceButton.onClick.RemoveAllListeners();
            rollDiceButton.onClick.AddListener(EndTurn);

            // currentPlayerNum is increased by 1 and loops back to the first player after the last player has had their turn
            //currentPlayerNum = (currentPlayerNum + 1) % board.GetNumPlayers();
        }
        else
        {
            currentPlayer.SetMoving(true);
            rollDiceButton.interactable = false;
        }
        
    }

    /// <summary>
    /// Performs the action for the tile the player is currently on
    /// </summary>
    private void TileAction()
    {
        BoardTile currentTile = board.GetBoardTiles()[currentPlayer.GetCurrentPosition()];

        currentTile.PerformAction(currentPlayer);
    }

    /// <summary>
    /// Sets the roll dice button's functionality to end the turn for the player
    /// </summary>
    public void SetButtonToEndTurn()
    {
        rollDiceButton.GetComponentInChildren<TMP_Text>().text = "End Turn";
        rollDiceButton.onClick.RemoveAllListeners();
        rollDiceButton.onClick.AddListener(EndTurn);
    }

    /// <summary>
    /// Activates popup at the end of a game
    /// </summary>
    public void EndGame()
    {
        Debug.Log("The game has ended");
        rollDiceButton.interactable = false;
        GameEndPopup.SetActive(true);
    }

    private void UpdateSellPropertyButton(Property property)
    {
        bool allZero = true;

        foreach (Property prop in currentPlayer.GetSameGroupProperties(property))
        {
            if (prop.GetNumHouses() > 0)
            {
                allZero = false;
            }
        }

        if (allZero)
        {
            sellProperty.interactable = true;
        }
        else
        {
            sellProperty.interactable = false;
        }
    }

    private void UpdateTaxButtons()
    {
        /*
        if (board.GetBoardTiles()[currentPlayer.GetCurrentPosition()].GetType() == typeof(Tax))
        {
            Tax tax = (Tax) board.GetBoardTiles()[currentPlayer.GetCurrentPosition()];
            if (tax.taxAmount > currentPlayer.GetMoney())
            {
                taxButton.interactable = false;
            }
            else
            {
                taxButton.interactable = true;
            }
        }
        */
        if (taxPopup.activeInHierarchy)
        {
            Tax tax = (Tax) board.GetBoardTiles()[currentPlayer.GetCurrentPosition()];
            if (tax.taxAmount > currentPlayer.GetMoney())
            {
                taxButton.interactable = false;
            }
            else
            {
                taxButton.interactable = true;
            }
        }

    }

    /// <summary>
    /// Updates the card buttons interactability
    /// </summary>
    private void UpdateCardButtons()
    {
        // If on an opportunity knocks space
        if (board.GetBoardTiles()[currentPlayer.GetCurrentPosition()].GetType() == typeof(OpportunityKnocks))
        {
            // Gets the current card from the front of the queue
            Card currentCard = board.GetOpportunityKnocksCards().Peek();
            Type cardType = board.GetOpportunityKnocksCards().Peek().GetType();

            if (cardType == typeof(LoseMoney))
            {
                LoseMoney card = (LoseMoney)currentCard;
                // Checks if the player has enough money to afford the fine on the card
                opportunityKnocksOkButton.interactable = currentPlayer.GetMoney() >= card.amount;
            }
            else if (cardType == typeof(Repairs))
            {
                Repairs card = (Repairs)currentCard;
                // Checks if the player has enough money to afford the fine on the card
                opportunityKnocksOkButton.interactable = currentPlayer.GetMoney() >= card.GetTotalCost(currentPlayer);
            }
        }
        // If on a potluck space
        else if (board.GetBoardTiles()[currentPlayer.GetCurrentPosition()].GetType() == typeof(PotLuck))
        {
            // Gets the current card from the front of the queue
            Card currentCard = board.GetPotluckCards().Peek();
            Type cardType = board.GetPotluckCards().Peek().GetType();

            if (cardType == typeof(LoseMoney))
            {
                LoseMoney card = (LoseMoney)currentCard;
                // Checks if the player has enough money to afford the fine on the card
                potLuckOkButton.interactable = currentPlayer.GetMoney() >= card.amount;
            }
            else if (cardType == typeof(Birthday))
            {
                Birthday card = (Birthday)currentCard;
                // Checks if the player has enough money to afford the fine on the card
                potLuckOkButton.interactable = currentPlayer.GetMoney() >= card.amount;
            }
            else if (cardType == typeof(Choice))
            {
                Choice card = (Choice)currentCard;
                // Checks if the player has enough money to afford the fine on the card
                potLuckOkButton.interactable = currentPlayer.GetMoney() >= card.amount;
            }
        }
    }

    /// <summary>
    /// Updates the property buttons interactability
    /// </summary>
    private void UpdatePropertyButtons()
    {
        // If on a property space
        if (board.GetBoardTiles()[currentPlayer.GetCurrentPosition()].GetType() == typeof(Property))
        {
            Property currentProperty = (Property)board.GetBoardTiles()[currentPlayer.GetCurrentPosition()];
            // Sets button to be interactable if the player can afford to pay the rent on it
            if (currentPlayer.GetMoney() >= currentProperty.rent[currentProperty.GetNumHouses()])
            {
                propertyPayPlayerButton.interactable = true;
            }
            else
            {
                propertyPayPlayerButton.interactable = false;
            }

            // Sets button to be interactable if the player can afford to buy the property
            if (currentPlayer.GetMoney() >= currentProperty.cost)
            {
                propertyPayPlayerButton.interactable = true;
            }
            else
            {
                propertyPayPlayerButton.interactable = false;
            }
        }
        // If on a station space
        else if (board.GetBoardTiles()[currentPlayer.GetCurrentPosition()].GetType() == typeof(Station))
        {
            Station currentProperty = (Station)board.GetBoardTiles()[currentPlayer.GetCurrentPosition()];
            // Sets button to be interactable if the player can afford to pay the rent on it
            if (currentPlayer.GetMoney() >= currentProperty.rent[currentProperty.GetNumStationsOwned()])
            {
                propertyPayPlayerButton.interactable = true;
            }
            else
            {
                propertyPayPlayerButton.interactable = false;
            }
        }
        // If on a utility space
        else if (board.GetBoardTiles()[currentPlayer.GetCurrentPosition()].GetType() == typeof(Utility))
        {
            Utility currentProperty = (Utility)board.GetBoardTiles()[currentPlayer.GetCurrentPosition()];
            // Sets button to be interactable if the player can afford to pay the rent on it
            if (currentPlayer.GetMoney() >= currentProperty.rent[currentProperty.GetNumUtilitiesOwned()])
            {
                propertyPayPlayerButton.interactable = true;
            }
            else
            {
                propertyPayPlayerButton.interactable = false;
            }
        }

        // If trying to buy a property
        if (board.GetBoardTiles()[currentPlayer.GetCurrentPosition()].GetType() == typeof(BuyableTile))
        {
            BuyableTile currentProperty = (BuyableTile) board.GetBoardTiles()[currentPlayer.GetCurrentPosition()];
            // Sets button to be interactable if the player can afford to buy the property
            if (currentPlayer.GetMoney() >= currentProperty.cost)
            {
                buyProperty.interactable = true;
            }
            else
            {
                buyProperty.interactable = false;
            }
        }


    }

    /// <summary>
    /// Shows the property panel.
    /// Allows the player to buy houses, sell houses, mortgage properties and sell properties
    /// </summary>
    public void ShowProperties()
    {
        viewProperties.GetComponent<Button>().onClick.RemoveAllListeners();
        // Checks if the panel is open already
        if (viewProperties.GetComponentInChildren<TMP_Text>().text == "View Properties")
        {
            viewPropertiesMenu.SetActive(true);

            viewProperties.GetComponentInChildren<TMP_Text>().text = "Back";
            // Sets the button to close the panel and destroy any objects that were created
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

            // Sorts all the properties a player owns based on their position on the board, lowest to highest
            sortedProperties.Sort((x, y) => x.position.CompareTo(y.position));
            // Loops for each property the player owns
            foreach (BuyableTile tile in sortedProperties)
            {
                // Added to a seperate list if the property is a station
                if (tile.GetType() == typeof(Station))
                {
                    stations.Add((Station)tile);
                }
                // Added to another seperate list if the property is a utility
                else if (tile.GetType() == typeof(Utility))
                {
                    utilities.Add((Utility)tile);
                }
                else
                {
                    Property property = (Property) tile;
                    // Creates a new button for the current property 
                    GameObject square = Instantiate(propertySquare);
                    square.GetComponentInChildren<TMP_Text>().text = property.GetNumHouses().ToString();
                    square.GetComponent<Image>().color = property.GetGroupColour();

                    square.GetComponent<Button>().onClick.RemoveAllListeners();
                    // When clicking on the button
                    square.GetComponent<Button>().onClick.AddListener(delegate
                    {
                        // Removes any listeners previously added to avoid adding multiple to one button
                        mortgage.onClick.RemoveAllListeners();
                        buyHouse.onClick.RemoveAllListeners();
                        sellHouse.onClick.RemoveAllListeners();
                        sellProperty.onClick.RemoveAllListeners();

                        // Sets all the info for the property on the info panel on the right
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

                        // When pressing the mortgage button
                        mortgage.onClick.AddListener(delegate
                        {
                            if (property.IsMortgaged())
                            {
                                property.SetMortgaged(false);
                                mortgage.GetComponentInChildren<TMP_Text>().text = "Mortgage";

                                // Sets buy house button to be active if the player has a full group set
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

                            // Updates button for when landing on another players property if you can then afford it after mortaging a house
                            UpdatePropertyButtons();
                            // Updates the pay to get out of jail button
                            payOutOfJailButton.interactable = currentPlayer.GetMoney() >= 50;
                            // Updates the card buttons
                            UpdateCardButtons();
                            UpdateTaxButtons();
                            UpdateSellPropertyButton(property);
                        });

                        // When pressing the buy house button
                        buyHouse.onClick.AddListener(delegate
                        {
                            // Buys the a house for the property
                            currentPlayer.BuyHouse(property);
                            sellHouse.interactable = true;
                            mortgage.interactable = false;

                            // Can't buy another house if the property has the max number of houses on it or
                            // unequal amounts of houses on other properties in the same group
                            if (property.GetNumHouses() >= 5 || currentPlayer.HasUnequalHouseAmounts(property, true))
                            {
                                buyHouse.interactable = false;
                            }

                            // Can't buy another house if the player cannot afford it
                            if (currentPlayer.GetMoney() < property.GetHousePrice())
                            {
                                buyHouse.interactable = false;
                            }
                            // Updates the number that tells the player how many houses the property has on it
                            square.GetComponentInChildren<TMP_Text>().text = property.GetNumHouses().ToString();

                            // Updates all the other buttons that you may be able to press 
                            UpdatePropertyButtons();
                            payOutOfJailButton.interactable = currentPlayer.GetMoney() >= 50;
                            UpdateCardButtons();
                            UpdateTaxButtons();
                            UpdateSellPropertyButton(property);
                        });

                        // When pressing the sell house button
                        sellHouse.onClick.AddListener(delegate
                        {
                            currentPlayer.SellHouse(property);
                            buyHouse.interactable = true;

                            // Can't sell another house if the property has the min number of houses on it or
                            // unequal amounts of houses on other properties in the same group
                            if (property.GetNumHouses() <= 0 || currentPlayer.HasUnequalHouseAmounts(property, false))
                            {
                                sellHouse.interactable = false;
                            }

                            // Can mortgage a property if it has no houses on it
                            if (property.GetNumHouses() <= 0)
                            {
                                mortgage.interactable = true;
                            }
                            // Updates the number that tells the player how many houses the property has on it
                            square.GetComponentInChildren<TMP_Text>().text = property.GetNumHouses().ToString();

                            // Updates all the other buttons that you may be able to press 
                            UpdatePropertyButtons();
                            payOutOfJailButton.interactable = currentPlayer.GetMoney() >= 50;
                            UpdateCardButtons();
                            UpdateTaxButtons();
                            UpdateSellPropertyButton(property);
                        });

                        // When pressing the sell property button
                        sellProperty.onClick.AddListener(delegate
                        {
                            currentPlayer.SellProperty(property);
                            Destroy(square.gameObject);
                            propertyInfoPanel.SetActive(false);


                            // Updates all the other buttons that you may be able to press 
                            currentPlayer.GetPropertyInfoSquare(property).GetComponent<Image>().color = new Color32(200, 200, 200, 255);
                            UpdatePropertyButtons();
                            payOutOfJailButton.interactable = currentPlayer.GetMoney() >= 50;
                            UpdateCardButtons();
                            UpdateTaxButtons();
                        });

                        // Can't buy a house if you don't have a full set, or can't afford it, or it is mortgaged, or it has the max number of houses on it,
                        // or has unequal amounts of houses across the set
                        if (currentPlayer.GetSameGroupProperties(property).Count != property.groupAmount
                            || currentPlayer.GetMoney() < property.GetHousePrice()
                            || property.IsMortgaged()
                            || property.GetNumHouses() >= 5
                            || currentPlayer.HasUnequalHouseAmounts(property, true))
                        {
                            buyHouse.interactable = false;
                        }
                        else
                        {
                            buyHouse.interactable = true;
                        }

                        // Can't sell a house if the property has no houses on it or has an unequal amount of houses on it
                        if (property.GetNumHouses() <= 0 || currentPlayer.HasUnequalHouseAmounts(property, false))
                        {
                            sellHouse.interactable = false;
                        }
                        else
                        {
                            sellHouse.interactable = true;
                        }

                        // Can't mortgage a property if it has any amount of houses on it
                        // Can't unmortgage a property if you don't have enough money
                        if (property.GetNumHouses() > 0 || (property.IsMortgaged() && (property.GetCost() / 2) > currentPlayer.GetMoney()))
                        {
                            mortgage.interactable = false;
                        }
                        else
                        {
                            mortgage.interactable = true;
                        }

                        UpdateSellPropertyButton(property);

                        // Sets all the gameobjects to be active
                        buyHouse.gameObject.SetActive(true);
                        sellHouse.gameObject.SetActive(true);
                        propertyInfoPanel.SetActive(true);
                    });
                    square.transform.SetParent(propertySquareHolder.transform, false);
                }
            }

            // For all the station that the player owns
            foreach (Station station in stations)
            {
                // Creates a new button for the current property 
                GameObject square = Instantiate(propertySquare);
                square.GetComponentInChildren<TMP_Text>().text = "";
                square.GetComponent<Image>().color = station.GetGroupColour();
                // When clicking the button
                square.GetComponent<Button>().onClick.AddListener(delegate
                {
                    mortgage.onClick.RemoveAllListeners();
                    sellProperty.onClick.RemoveAllListeners();

                    // Sets all the info for the property on the info panel on the right
                    propertyInfoPanelName.text = station.tileName;

                    propertyInfoPanelRent.text = "";
                    for (int i = 0; i < 4; i++)
                    {
                        propertyInfoPanelRent.text += (station.rent[i] + "\n");
                    }

                    // When pressing the mortgage button
                    mortgage.onClick.AddListener(delegate
                    {
                        if (station.IsMortgaged())
                        {
                            // Unmortgages if was mortgaged
                            station.SetMortgaged(false);
                            mortgage.GetComponentInChildren<TMP_Text>().text = "Mortgage";
                        }
                        else
                        {
                            // Mortgages if was not mortgaged
                            station.SetMortgaged(true);
                            mortgage.GetComponentInChildren<TMP_Text>().text = "Unmortgage";
                        }

                        // Updates all the other buttons that you may be able to press 
                        UpdatePropertyButtons();
                        payOutOfJailButton.interactable = currentPlayer.GetMoney() >= 50;
                        UpdateCardButtons();
                        UpdateTaxButtons();
                    });

                    // When pressing the sell property button
                    sellProperty.onClick.AddListener(delegate
                    {
                        // Updates all the other buttons that you may be able to press 
                        UpdatePropertyButtons();
                        payOutOfJailButton.interactable = currentPlayer.GetMoney() >= 50;
                        UpdateCardButtons();
                        UpdateTaxButtons();

                        currentPlayer.GetPropertyInfoSquare(station).GetComponent<Image>().color = new Color32(200, 200, 200, 255);
                        currentPlayer.SellProperty(station);
                        Destroy(square.gameObject);
                        propertyInfoPanel.SetActive(false);
                    });

                    // Sets the text for if mortgaged or not mortgaged
                    if (station.IsMortgaged())
                    {
                        mortgage.GetComponentInChildren<TMP_Text>().text = "Unmortgage";
                    }
                    else
                    {
                        mortgage.GetComponentInChildren<TMP_Text>().text = "Mortgage";
                    }

                    // Can't unmortgage a station if you don't have enough money
                    if (station.IsMortgaged() && (station.GetCost() / 2) > currentPlayer.GetMoney())
                    {
                        mortgage.interactable = false;
                    }
                    else
                    {
                        mortgage.interactable = true;
                    }

                    buyHouse.gameObject.SetActive(false);
                    sellHouse.gameObject.SetActive(false);
                    propertyInfoPanel.SetActive(true);
                });
                square.transform.SetParent(propertySquareHolder.transform, false);
            }
            foreach (Utility utility in utilities)
            {
                // Creates a new button for the current property 
                GameObject square = Instantiate(propertySquare);
                square.GetComponentInChildren<TMP_Text>().text = "";
                square.GetComponent<Image>().color = utility.GetGroupColour();
                // When clicking the button
                square.GetComponent<Button>().onClick.AddListener(delegate
                {
                    mortgage.onClick.RemoveAllListeners();
                    sellProperty.onClick.RemoveAllListeners();

                    // Sets all the info for the property on the info panel on the right
                    propertyInfoPanelName.text = utility.tileName;

                    propertyInfoPanelRent.text = "";
                    for (int i = 0; i < 2; i++)
                    {
                        propertyInfoPanelRent.text += (utility.rent[i] + "\n");
                    }

                    // When pressing the mortgage button
                    mortgage.onClick.AddListener(delegate
                    {
                        if (utility.IsMortgaged())
                        {
                            // Unmortgages if was mortgaged
                            utility.SetMortgaged(false);
                            mortgage.GetComponentInChildren<TMP_Text>().text = "Mortgage";
                        }
                        else
                        {
                            // Mortgages if was not mortgaged
                            utility.SetMortgaged(true);
                            mortgage.GetComponentInChildren<TMP_Text>().text = "Unmortgage";
                        }

                        // Updates all the other buttons that you may be able to press 
                        UpdatePropertyButtons();
                        payOutOfJailButton.interactable = currentPlayer.GetMoney() >= 50;
                        UpdateCardButtons();
                        UpdateTaxButtons();
                    });

                    // When pressing the sell property button
                    sellProperty.onClick.AddListener(delegate
                    {
                        // Updates all the other buttons that you may be able to press 
                        UpdatePropertyButtons();
                        payOutOfJailButton.interactable = currentPlayer.GetMoney() >= 50;
                        UpdateCardButtons();
                        UpdateTaxButtons();

                        currentPlayer.GetPropertyInfoSquare(utility).GetComponent<Image>().color = new Color32(200, 200, 200, 255);
                        currentPlayer.SellProperty(utility);
                        Destroy(square.gameObject);
                        propertyInfoPanel.SetActive(false);
                    });

                    // Sets the text for if mortgaged or not mortgaged
                    if (utility.IsMortgaged())
                    {
                        mortgage.GetComponentInChildren<TMP_Text>().text = "Unmortgage";
                    }
                    else
                    {
                        mortgage.GetComponentInChildren<TMP_Text>().text = "Mortgage";
                    }

                    // Can't unmortgage a utility if you don't have enough money
                    if (utility.IsMortgaged() && (utility.GetCost() / 2) > currentPlayer.GetMoney())
                    {
                        mortgage.interactable = false;
                    }
                    else
                    {
                        mortgage.interactable = true;
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
