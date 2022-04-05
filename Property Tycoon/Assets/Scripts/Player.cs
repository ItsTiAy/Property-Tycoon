using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
	private int numLaps;
    private int currentPosition = 0;
    private int newPosition;
    private const int numSpacesOnBoard = 40;
    private bool moving = false;
	private int diceRollNum;
    public Transform[] waypoints;
	public GameObject playerInfoCards;
	public GameObject infoCard;
	private GameObject playerInfoCard;
	private TMP_Text moneyUI;

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
    
    public void UpdatePosition(int diceNum)
    {
		// Increases the players position by the value on the dice, loops back to start when greater than 40
		newPosition = (currentPosition + diceNum) % numSpacesOnBoard;
    }

	public bool MovePlayerPiece()
	{
		bool finished = false;

		if (currentPosition != newPosition)
		{
			if (transform.position != waypoints[(currentPosition + 1) % numSpacesOnBoard].transform.position)
			{
				transform.position = Vector3.MoveTowards(transform.position, waypoints[(currentPosition + 1) % numSpacesOnBoard].transform.position, 20f * Time.deltaTime);
			}
			else
			{
				currentPosition = (currentPosition + 1) % numSpacesOnBoard;
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
			moving = false;
			finished = true;
		}
		return finished;
	}

	public Transform GetPropertyInfoSquare(BuyableTile tile)
    {
		Transform properties = playerInfoCard.transform.GetChild(2);
		for (int i = 0; i < properties.childCount; i++)
        {
			Transform group = properties.GetChild(i);
			if (tile.group.ToLower() == group.name.ToLower())
			{
				for (int j = 0; j < group.childCount; j++)
				{
					if (tile.position.ToString().Equals(group.GetChild(j).name))
					{
						return group.GetChild(j);
					}
				}
			}
		}
		Debug.Log("Something went wrong");
		return null;
    }

	public int GetNumLaps()
    {
		return numLaps;
    }

	public void SetMoving(bool moving)
    {
		this.moving = moving;
    }

	public int GetDiceRollValue()
    {
		return diceRollNum;
    }

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

    public bool IsMoving()
    {
        return moving;
    }
    
    public void setWaypoints(Transform[] waypoints)
    {
        this.waypoints = waypoints;
    }

	public int GetMoney()
	{
		return money;
	}

	public Transform[] GetWaypoints()
    {
        return waypoints;
    }

	public void BuyProperty(BuyableTile property)
    {
		properties.Add(property);
		property.SetBuyable(false);
		property.SetOwner(this);
		DecreaseMoney(property.GetCost());
    }

	public void SellProperty(Property property)
	{
		properties.Remove(property);
		property.SetBuyable(true);
		property.SetOwner(null);
		IncreaseMoney(property.GetCost());
	}

	public void BuyHouse(Property property)
    {
		property.AddHouse();
		money -= property.GetHousePrice();
    }

	public void SellHouse(Property property)
	{
		property.RemoveHouse();
		money += property.GetHousePrice();
	}

	public void MortgageProperty(Property property)
	{
		property.SetMortgaged(true);
		IncreaseMoney(property.GetCost() / 2);
	}

	public void UnmortgageProperty(Property property)
	{
		property.SetMortgaged(false);
		DecreaseMoney(property.GetCost() / 2);
	}

	public int calculateMaxPossibleMoney()
	{
		int m = money;

		properties.ForEach(i => Debug.Log(i.ToString()));

		foreach (BuyableTile tile in properties)
        {
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
			else //if (tile.GetType() == typeof(Station) || tile.GetType() == typeof(Utility))
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

	public bool DecreaseMoney(int amount)
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
			money = -1;
			return false;
		}
		return true; //player has managed to pay fine
	}

	public void IncreaseMoney(int amount)
	{ 
		//tested
		money += amount;
		moneyUI.text = "£" + money;
	}
}