using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Stores infomation and methods for the player
/// </summary>
public class Player : MonoBehaviour
{
	private int numLaps = 1;
    private int currentPosition = 0;
    private int newPosition;
	private int turnsInJail;
    private const int numSpacesOnBoard = 40;
    private bool moving = false;
	private int diceRollNum;
	private int numDoubles = 0;
    public Transform[] waypoints;
	public GameObject playerInfoCards;
	public GameObject infoCard;
	private GameObject playerInfoCard;
	private TMP_Text moneyUI;
	private Card getOutOfJailFreeCardPotLuck;
	private Card getOutOfJailFreeCardOpportunityKnocks;
	private Transform jailWaypoint;
	private bool jail;

	public GameObject button;
    private Board board;

	// Should be private, add getters and setters later
	public List<BuyableTile> properties;
	public List<Property> mortgaged;
	public int money;

	public Player()
	{ 
		//Constructor
	}


	private void Start()
    {
        button = GameObject.Find("Roll Dice");
		playerInfoCards = GameObject.Find("PlayerInfoCards");
		playerInfoCard = Instantiate(infoCard, Vector3.zero, Quaternion.identity);
		playerInfoCard.transform.SetParent(playerInfoCards.transform, false);

		moneyUI = playerInfoCard.transform.GetChild(1).GetComponent<TMP_Text>();
		properties = new List<BuyableTile>();
		mortgaged = new List<Property>();
		money = 1500;
	}

	/// <summary>
	/// Updates the players position by the value
	/// </summary>
	/// <param name="diceNum"></param>
	public void UpdatePosition(int diceNum)
    {
		// Increases the players position by the value on the dice, loops back to start when greater than 40
		newPosition = (currentPosition + diceNum) % numSpacesOnBoard;
    }

	/// <summary>
	/// Sets the player's position to the specified position
	/// and moves the player's position on the UI board
	/// </summary>
	/// <param name="tilePosition"> The position that the player will be set to</param>
	public void SetPosition(int tilePosition)
    {
		currentPosition = tilePosition - 1;
		newPosition = currentPosition;
		transform.position = waypoints[currentPosition].transform.position;
    }

	/// <summary>
	/// Sends the player to jail
	/// </summary>
	public void GoToJail()
    {
		jail = true;
		currentPosition = 10;
		newPosition = currentPosition;
		board.GetBoardTiles()[10].PerformAction(this);
		transform.position = jailWaypoint.transform.position;
		board.EndTurn();
	}

	/// <summary>
	/// Moves the player out of jail
	/// </summary>
	public void GetOutOfJail()
    {
		jail = false;
		transform.position = waypoints[10].transform.position;
	}

	/// <summary>
	/// Returns if the player is in jail
	/// </summary>
	/// <returns>True if the player is in jail, false if not</returns>
	public bool InJail()
    {
		return jail;
    }

	/// <summary>
	/// Returns the number of turns the player has spent in jail
	/// </summary>
	/// <returns>The number of turns the player has spent in jail</returns>
	public int GetTurnsInJail()
    {
		return turnsInJail;
    }

	/// <summary>
	/// Increases the number of turns a player has spent in jail
	/// </summary>
	public void IncreaseTurnsInJail()
    {
		turnsInJail++;
    }

	/// <summary>
	/// Resets the number of turns a player has spent in jail
	/// </summary>
	public void ResetTurnsInJail()
    {
		turnsInJail = 0;
    }

	/// <summary>
	/// Increases the number of doubles a player has had in a row
	/// </summary>
	public void IncreaseNumDoubles()
    {
		numDoubles++;
    }

	/// <summary>
	/// Returns the number of doubles a player has had in a row
	/// </summary>
	/// <returns>The number of doubles a player has had in a row</returns>
	public int GetNumDoubles()
    {
		return numDoubles;
    }

	/// <summary>
	/// Resets the the number of doubles a player has had in a row
	/// </summary>
	public void ResetNumDoubles()
    {
		numDoubles = 0;
    }

	/// <summary>
	/// Moves the players board piece around the board
	/// </summary>
	/// <returns>True when the player has finished moving</returns>
	public bool MovePlayerPiece()
	{
		bool finished = false;

		// Checks if the current pos is at the new pos
		if (currentPosition != newPosition)
		{
			// If the player has not reached the next space on the board
			if (transform.position != waypoints[(currentPosition + 1) % numSpacesOnBoard].transform.position)
			{
				// Moves the player towards the next space on the board
				transform.position = Vector3.MoveTowards(transform.position, waypoints[(currentPosition + 1) % numSpacesOnBoard].transform.position, 20f * Time.deltaTime);
			}
			else
			{
				// Increases the current position by 1
				currentPosition = (currentPosition + 1) % numSpacesOnBoard;
				// Checks if passed go
				if (currentPosition == 0)
                {
					Debug.Log("Passed Go");
					IncreaseMoney(200);
					numLaps++;
                }
			}
		}
		else
		{
			// Once current pos equals the new pos
			moving = false;
			finished = true;
		}
		return finished;
	}

	/// <summary>
	/// Finds the info square (the square on the small card that each player has) on the UI that correlates to the tile parameter
	/// </summary>
	/// <param name="tile">The property thats square needs to be found</param>
	/// <returns>The property square gameobject</returns>
	public Transform GetPropertyInfoSquare(BuyableTile tile)
    {
		Transform properties = playerInfoCard.transform.GetChild(2);
		// Loops through all the differnt groups of properties
		for (int i = 0; i < properties.childCount; i++)
        {
			Transform group = properties.GetChild(i);
			// If the group matches
			if (tile.group.ToLower() == group.name.ToLower())
			{
				// Loops through all the properties in that group
				for (int j = 0; j < group.childCount; j++)
				{
					if (tile.position.ToString().Equals(group.GetChild(j).name))
					{
						// Returns the object that's name matches the position of the tile
						return group.GetChild(j);
					}
				}
			}
		}
		Debug.Log("Something went wrong");
		// Should never end up here, should always find the tile
		return null;
    }

	/// <summary>
	/// Sets the card for the potluck get of jail free card
	/// </summary>
	/// <param name="getOutOfJailFree">The get out of jail free card</param>
	public void setGetOutOfJailFreePotluck(Card getOutOfJailFree)
    {
		getOutOfJailFreeCardPotLuck = getOutOfJailFree;
    }

	/// <summary>
	/// Sets the card for the opportunity knocks get of jail free card
	/// </summary>
	/// <param name="getOutOfJailFree">The get out of jail free card</param>
	public void setGetOutOfJailFreeOpportunityKnocks(Card getOutOfJailFree)
	{
		getOutOfJailFreeCardOpportunityKnocks = getOutOfJailFree;
	}

	/// <summary>
	/// Returns the card for the potluck get of jail free card
	/// </summary>
	/// <returns>The card for the potluck get of jail free card</returns>
	public Card GetGetOutOfJailFreePotLuck()
	{
		return getOutOfJailFreeCardPotLuck;
	}

	/// <summary>
	/// Returns the card for the opportunity knocks get of jail free card
	/// </summary>
	/// <returns>The card for the opportunity knocks get of jail free card</returns>
	public Card GetGetOutOfJailFreeOpportunityKnocks()
	{
		return getOutOfJailFreeCardOpportunityKnocks;
	}

	/// <summary>
	/// Checks if the player has the opportunity knocks get of jail free card
	/// </summary>
	/// <returns>True if the player has the card, false if not</returns>
	public bool HasGetOutOfJailFreeOpportunityKnocks()
    {
		return getOutOfJailFreeCardOpportunityKnocks != null;
	}

	/// <summary>
	/// Checks if the player has the potluck get of jail free card
	/// </summary>
	/// <returns>True if the player has the card, false if not</returns>
	public bool HasGetOutOfJailFreePotLuck()
	{
		return getOutOfJailFreeCardPotLuck != null;
	}

	/// <summary>
	/// Returns the number of laps the player has done
	/// </summary>
	/// <returns>The number of laps the player has done</returns>
	public int GetNumLaps()
    {
		return numLaps;
    }

	/// <summary>
	/// Sets the player moving to true or false
	/// </summary>
	/// <param name="moving">The value you want to set moving to</param>
	public void SetMoving(bool moving)
    {
		this.moving = moving;
    }

	/// <summary>
	/// Sets the board for the player so it can access it's methods
	/// </summary>
	/// <param name="board">The board you want to set the board to</param>
	public void SetBoard(Board board)
    {
		this.board = board;
    }

	/// <summary>
	/// Sets diceRollNum to the latest dice roll value
	/// </summary>
	/// <param name="num">The value you want to set diceRollNum to</param>
	public void SetDiceRollValue(int num)
	{
		diceRollNum = num;
	}

	/// <summary>
	/// Returns the value of diceRollNum
	/// </summary>
	/// <returns>The value of diceRollNum</returns>
	public int GetDiceRollValue()
    {
		return diceRollNum;
    }

	/// <summary>
	/// Returns the players current position
	/// </summary>
	/// <returns>The players current position</returns>
	public int GetCurrentPosition()
    {
        return currentPosition;
    }

    public void SetCurrentPosition(int currentPosition)
    {
        this.currentPosition = currentPosition;
    }

    public int GetNewPosition()
    {
        return newPosition;
    }

	/// <summary>
	/// Returns whether the player is moving or not
	/// </summary>
	/// <returns>True if the player is moving, false if not</returns>
	public bool IsMoving()
    {
        return moving;
    }

	/// <summary>
	/// Sets the waypoints that the player uses to move around the board
	/// </summary>
	/// <param name="waypoints">The list of waypoints you want to set the waypoints to</param>
	public void setWaypoints(Transform[] waypoints)
    {
        this.waypoints = waypoints;
    }

	/// <summary>
	/// Returns the amount of money the player has
	/// </summary>
	/// <returns>The amount of money the player has</returns>
	public int GetMoney()
	{
		return money;
	}

	public Transform[] GetWaypoints()
    {
        return waypoints;
    }

	/// <summary>
	/// Sets the waypoint that the player uses when being sent to jail
	/// </summary>
	/// <param name="waypoint">The waypoint that you want to set the jail waypoint to</param>
	public void SetJailWaypoint(Transform waypoint)
    {
		jailWaypoint = waypoint;

	}

	/// <summary>
	/// Buys a property
	/// </summary>
	/// <param name="property">The property to buy</param>
	public void BuyProperty(BuyableTile property)
    {
		properties.Add(property);
		property.SetBuyable(false);
		property.SetOwner(this);
		DecreaseMoney(property.GetCost());
    }

	/// <summary>
	/// Sells a property
	/// </summary>
	/// <param name="property">The property to sell</param>
	public void SellProperty(BuyableTile property)
	{
		properties.Remove(property);
		property.SetBuyable(true);
		property.SetOwner(null);

		if (property.IsMortgaged())
        {
			IncreaseMoney(property.GetCost() / 2);
			Debug.Log(money);
		}
		else
        {
			IncreaseMoney(property.GetCost());
			Debug.Log(money);
		}

		property.SetMortgaged(false);
	}

	/// <summary>
	/// Buy a house for a property
	/// </summary>
	/// <param name="property">The property you want to buy a house for</param>
	public void BuyHouse(Property property)
    {
		property.AddHouse();
		DecreaseMoney(property.GetHousePrice());
    }

	/// <summary>
	/// Sell a house for a property
	/// </summary>
	/// <param name="property">The property you want to sell a house for</param>
	public void SellHouse(Property property)
	{
		property.RemoveHouse();
		IncreaseMoney(property.GetHousePrice());
	}
	/*
	public void MortgageProperty(Property property)
	{
		property.SetMortgaged(true);
	}

	public void UnmortgageProperty(Property property)
	{
		property.SetMortgaged(false);
	}
	*/
	/// <summary>
	/// Calculates if the player can afford to pay a fine if they sell everything they own
	/// </summary>
	/// <returns>The amount of money the player has if they sell everything</returns>
	public int calculateMaxPossibleMoney()
	{
		int m = money;

		// Loops for each property the player owns
		foreach (BuyableTile tile in properties)
        {
			// For properties
			if (tile.GetType() == typeof(Property))
            {
				Property property = (Property) tile;
				if (property.IsMortgaged())
				{
					m += (property.GetCost() / 2);
				}
				else
				{
					m += property.GetCost();
					m += property.GetNumHouses() * property.GetHousePrice();
				}
			}
			// For stations and utilities
			else
			{
				if (tile.IsMortgaged())
				{
					m += (tile.GetCost() / 2);
				}
				else
				{
					m += tile.GetCost();
				}
			}
		}

		return m;
		//Returns money + money from selling all houses + mortgaging
	}

	public bool hasProperty(Property p)
	{
		return properties.Contains(p);
	}

	/// <summary>
	/// Finds all the properties a player owns that are in the same group
	/// </summary>
	/// <param name="property">The property that's group you want to find all the other properties of</param>
	/// <returns>All the properties that are in the same as the parameter property</returns>
	public List<BuyableTile> GetSameGroupProperties(BuyableTile property)
    {
		List<BuyableTile> sameGroupProperties = new List<BuyableTile>();

		foreach (BuyableTile tile in properties)
        {
			if (property.group.ToLower() == tile.group.ToLower())
            {
				sameGroupProperties.Add(tile);
            }
        }

		return sameGroupProperties;
    }

	/// <summary>
	/// Checks if the houses in property parameters group are unequal
	/// </summary>
	/// <param name="property">The property that's groups houses you want to check are not equal</param>
	/// <param name="checkingMin">True when buying a house, false when selling a house</param>
	/// <returns>True if unequal amount of houses, false if equal</returns>
	public bool HasUnequalHouseAmounts(Property property, bool checkingMin)
    {
		List<BuyableTile> sameGroupProperties = GetSameGroupProperties(property);
		List<Property> otherProperties = new List<Property>();
		int leastHouses;
		int mostHouses;

		// Adds all properties in a group into a list except the property from the parameters
		foreach (Property tile in sameGroupProperties)
        {
			if (tile != property)
            {
				otherProperties.Add(tile);
            }
        }

		if (sameGroupProperties.Count == 3)
        {
			// Finds the least and most houses a property in the group has
			leastHouses = Mathf.Min(otherProperties[0].GetNumHouses(), otherProperties[1].GetNumHouses());
			mostHouses = Mathf.Max(otherProperties[0].GetNumHouses(), otherProperties[1].GetNumHouses());
		}
		else
        {
			leastHouses = otherProperties[0].GetNumHouses();
			mostHouses = otherProperties[0].GetNumHouses();
		}

		if (checkingMin)
        {
			// True if the property has more houses than the property in the group with the least houses
			return property.GetNumHouses() > leastHouses;
		}
		else
        {
			// True if the property has less houses than the property in the group with the most houses
			return property.GetNumHouses() < mostHouses;
		}

    }

	/// <summary>
	/// Decreases the players money
	/// </summary>
	/// <param name="amount">The amount you want to decrease the player's money by</param>
	public void DecreaseMoney(int amount)
	{ 
		//tested
		if (money >= amount)
		{
			money -= amount;
			moneyUI.text = "£" + money;
		}
		else if (calculateMaxPossibleMoney() >= amount)
		{
			//allow player to sell their properties
		}
		else
		{
			//if the player can't afford the fine even if selling everything
			//Bankrupt();
		}
	}


	/// <summary>
	/// Increases the players money
	/// </summary>
	/// <param name="amount">The amount you want to increase the player's money by</param>
	public void IncreaseMoney(int amount)	{ 
		//tested
		money += amount;
		moneyUI.text = "£" + money;
	}

	public void Bankrupt()
    {
		money = -1;
		board.bankruptButton.onClick.RemoveAllListeners();
		board.bankruptButton.onClick.AddListener(RemovePlayer);
		board.bankruptPopup.SetActive(true);
	}

	/// <summary>
	/// Removes the player from the game
	/// </summary>
	public void RemovePlayer()
    {
		// Sets all properties the player owned parameters back to their defaults
		foreach (BuyableTile tile in properties)
		{
			tile.SetOwner(null);
			tile.SetMortgaged(false);
			tile.SetBuyable(true);

			if (tile.GetType() == typeof(Property))
			{
				Property property = (Property)tile;
				// Removes all houses from a property if it has any
				while (property.GetNumHouses() != 0)
				{
					property.RemoveHouse();
				}
			}
		}

		// Hides all UI elements for this player
		playerInfoCard.SetActive(false);
		gameObject.SetActive(false);
		board.GetPlayers().Remove(gameObject);
	}
}