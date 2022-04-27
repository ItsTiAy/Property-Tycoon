using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class Utility : BuyableTile
{
	/// <summary>
	/// Returns the number of utilities that the player who owns the property has
	/// </summary>
	/// <returns>The number of utilities that the player who owns the property has</returns>
	public int GetNumUtilitiesOwned()
    {
		int numUtilitiesOwned = 0;
		// Loops through all the properties the player has
		foreach (BuyableTile tile in ownedBy.properties)
		{
			if (tile.GetType() == typeof(Utility))
			{
				numUtilitiesOwned++;
			}
		}
		return numUtilitiesOwned;
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
				for (int i = 0; i < 2; i++)
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
				// Checks if the player that owns the property is not in jail
				if (!ownedBy.InJail())
				{
					int numUtilitiesOwned = GetNumUtilitiesOwned();

					int amountToPay = rent[numUtilitiesOwned - 1] * currentPlayer.GetDiceRollValue();

					if (currentPlayer.calculateMaxPossibleMoney() < amountToPay)
					{
						// The current player goes bankrupt if they cannot afford the rent even if they sell everything
						currentPlayer.Bankrupt();
					}
					else
					{
						// Button not interactable if the player does not currently have enough to pay the rent 
						if (currentPlayer.GetMoney() < amountToPay)
						{
							otherPropertyText.text = "You do not have enough to pay the rent";
							okButton.interactable = false;
						}
						// Is interactable otherwise
						else
						{
							otherPropertyText.text = "You owe £" + amountToPay + " for landing on " + tileName;
							okButton.interactable = true;
						}

						okButton.onClick.RemoveAllListeners();
						// When pressing the ok button
						okButton.onClick.AddListener(delegate
						{
							currentPlayer.DecreaseMoney(amountToPay);
							ownedBy.IncreaseMoney(amountToPay);
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
}
