using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class Property : BuyableTile
{
	private int numHouses;
	public int houseCost;

	public Property()
	{

	} 

	public void AddHouse()
	{
		//Increases number of houses by 1
		numHouses++;

		if (numHouses > rent.Length - 1)
		{ 
			numHouses = rent.Length - 1;
		}
	}

	public void RemoveHouse()
	{
		//Decreases number of houses by 1
		numHouses--;

		if (numHouses < 0)
        {
			numHouses = 0;
        }
	}

	// Action performed when the tile is landed on
	public override void PerformAction(Player currentPlayer)
    {
		if (buyable)
		{
			// Sets the name on the popup
			propertyNameUI.text = tileName;

			// Sets the rent prices on the poup
			rentPricesUI.text = "";
			for (int i = 0; i < 6; i++)
			{
				rentPricesUI.text += (rent[i] + "\n");
			}

			// Sets buyable to false when the button is pressed
			buyButton.onClick.RemoveAllListeners();
			buyButton.onClick.AddListener(delegate
			{ 
				currentPlayer.BuyProperty(this);
				currentPlayer.GetPropertyInfoSquare(this).GetComponent<Image>().color = GetGroupColour();
				Debug.Log("Player bought " + tileName + " for £" + cost); 
			});

			// Can't click buy button if the player does not have enough money
			buyButton.interactable = currentPlayer.GetMoney() >= cost;

			// Shows the popup
			propertyPopup.SetActive(true);
		}
		else
		{
			if (currentPlayer != ownedBy)
			{
				// UI shenanigans here
				// Add check for if the player has to sell things to afford rent
				if (currentPlayer.GetMoney() < rent[numHouses])
				{
					Debug.Log("You do not have enough to pay the rent");
				}
				else
				{
					Debug.Log("Player payed £" + rent[numHouses] + " for landing on " + tileName);
					currentPlayer.DecreaseMoney(rent[numHouses]);
					ownedBy.IncreaseMoney(rent[numHouses]);
				}
			}
			rollDiceButton.interactable = true;
		}
	}

	public int GetNumHouses()
	{
		return numHouses;
	}

	public int GetHousePrice()
	{
		return houseCost;
	}
}