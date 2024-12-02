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

	/// <summary>
	/// Increases number of houses by 1
	/// </summary>
	public void AddHouse()
	{
		numHouses++;

		if (numHouses > rent.Length - 1)
		{ 
			numHouses = rent.Length - 1;
		}
	}

	/// <summary>
	/// Decreases number of houses by 1
	/// </summary>
	public void RemoveHouse() 
	{
		numHouses--;

		if (numHouses < 0)
        {
			numHouses = 0;
        }
	}

	public override void PerformAction(Player currentPlayer)
    {
		if (buyable)
		{
			if (currentPlayer.GetNumLaps() >= 1)
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
				rollDiceButton.interactable = true;
			}
		}
		else
		{
			// If the current player does not own the property
			if (currentPlayer != ownedBy)
			{
				int rentCost = rent[numHouses];

				// Checks if the player that owns the property has the full set
				if (ownedBy.GetSameGroupProperties(this).Count == groupAmount && numHouses == 0)
                {
					rentCost = rent[0] * 2;
                }

				// Checks if the player that owns the property is not in jail
				if (!ownedBy.InJail())
				{
					if (currentPlayer.calculateMaxPossibleMoney() < rentCost)
					{
						// The current player goes bankrupt if they cannot afford the rent even if they sell everything
						currentPlayer.Bankrupt();
					}
					else
					{
						// Button not interactable if the player does not currently have enough to pay the rent 
						if (currentPlayer.GetMoney() < rentCost)
						{
							otherPropertyText.text = "You do not have enough to pay the rent";
							okButton.interactable = false;
						}
						// Is interactable otherwise
						else
						{
							otherPropertyText.text = "You owe £" + rentCost + " for landing on " + tileName;
							okButton.interactable = true;
						}

						okButton.onClick.RemoveAllListeners();
						// When pressing the ok button
						okButton.onClick.AddListener(delegate
						{
							currentPlayer.DecreaseMoney(rentCost);
							ownedBy.IncreaseMoney(rentCost);
						});
						otherPropertyPopup.SetActive(true);
					}
				}
				// Nothing happens if player who owns the property is in jail
				else
                {
					rollDiceButton.interactable = true;
				}
			}
			// Nothing happens if player who owns the property is the current player
			else
			{
				rollDiceButton.interactable = true;
			}
		}
	}

	/// <summary>
	/// Returns the number of houses the property has
	/// </summary>
	/// <returns>The number of houses the property has</returns>
	public int GetNumHouses()
	{
		return numHouses;
	}

	/// <summary>
	/// Returns the cost to buy a house for this property
	/// </summary>
	/// <returns>The cost to buy a house for this property</returns>
	public int GetHousePrice()
	{
		return houseCost;
	}
}