using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class Utility : BuyableTile
{
	public override void PerformAction(Player currentPlayer)
	{
		if (buyable)
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
				currentPlayer.GetPropertyInfoSquare(this).GetComponent<Image>().color = Color.grey;
				Debug.Log("Player bought " + tileName + " for £" + cost);
			});

			// Can't click buy button if the player does not have enough money
			buyButton.interactable = currentPlayer.GetMoney() >= cost;

			// Shows the popup
			propertyPopup.SetActive(true);
		}
		else
		{
			// Code for if owned here
			if (currentPlayer != ownedBy)
			{
				int numUtilitiesOwned = 0;
				//ownedBy.properties.ForEach(i => Debug.Log(i.ToString()));
				foreach (BuyableTile tile in ownedBy.properties)
				{
					if (tile.GetType() == typeof(Utility))
                    {
						numUtilitiesOwned++;
                    }
				}

				//Debug.Log(numUtilitiesOwned);

				int amountToPay = rent[numUtilitiesOwned - 1] * currentPlayer.GetDiceRollValue();

				Debug.Log("Player payed £" + amountToPay + " for landing on " + tileName);

				// UI shenanigans here
				currentPlayer.DecreaseMoney(amountToPay);
				ownedBy.IncreaseMoney(amountToPay);
			}
			rollDiceButton.interactable = true;
		}
	}
}
