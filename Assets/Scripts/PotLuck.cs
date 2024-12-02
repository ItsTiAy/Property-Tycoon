using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class PotLuck : BoardTile
{
	private GameObject potLuckPopup;
	private Button rollDiceButton;
	private TMP_Text description;
	private Button okButton;
	private Button takeCardButton;

	public Queue<Card> potLuckCards;

	private Card currentCard;

	/// <summary>
	/// Sets Objects, text and buttons to be used by this class
	/// </summary>
	/// <param name="potLuckPopup">The opportunity knocks popup</param>
	/// <param name="rollDiceButton">The roll dice button</param>
	/// <param name="description">The popup description</param>
	/// <param name="okButton">The popup ok button</param>
	/// <param name="takeCardButton">The button for taking a card</param>
	public void setObjects(GameObject potLuckPopup, Button rollDiceButton, TMP_Text description, Button okButton, Button takeCardButton)
    {
		this.potLuckPopup = potLuckPopup;
		this.rollDiceButton = rollDiceButton;
		this.description = description;
		this.okButton = okButton;
		this.takeCardButton = takeCardButton;
	}

	public override void PerformAction(Player currentPlayer)
	{
		potLuckCards = board.GetPotluckCards();
		// Sets the current card
		currentCard = potLuckCards.Peek();
		// Sets the description in the popup
		description.text = currentCard.description;

		okButton.onClick.RemoveAllListeners();

		if (currentCard.GetType() != typeof(MoveForward)
			&& currentCard.GetType() != typeof(MoveBackward)
			&& currentCard.GetType() != typeof(MoveBackwardAmount)
			)
		{
			okButton.onClick.AddListener(delegate
			{
				// Adds an extra listener to the ok button to make the roll dice button interactable if the card is not a
				// move forward, move backward, or move backwards amount card
				rollDiceButton.interactable = true;
			});
		}

		// If a choice card
		if (currentCard.GetType() == typeof(Choice))
		{
			okButton.GetComponentInChildren<TMP_Text>().text = "Pay Fine";
			// Adds extra button for the choice
			takeCardButton.gameObject.SetActive(true);

			Choice card = (Choice) currentCard;

			// Sets the pay fine button to not be interactable if the player cannot afford it
			if (card.amount > currentPlayer.GetMoney())
			{
				okButton.interactable = false;
			}

			takeCardButton.onClick.RemoveAllListeners();
			// When pressing the take card button
			takeCardButton.onClick.AddListener(delegate
			{
				// Performs the action of an opportunity knocks space
				board.GetBoardTiles()[7].PerformAction(currentPlayer);
				// Removes the extra button
				takeCardButton.gameObject.SetActive(false);
				// Resets the button text
				okButton.GetComponentInChildren<TMP_Text>().text = "Ok";
			});
		}

		// When pressing the ok button
		okButton.onClick.AddListener(delegate
		{
			currentCard.performCard(currentPlayer);
			takeCardButton.gameObject.SetActive(false);
			okButton.GetComponentInChildren<TMP_Text>().text = "Ok";
			potLuckCards.Dequeue();
		});

		// Gives the card to the current player if it is a get out of jail free card
		if (currentCard.GetType() == typeof(GetOutOfJailFree))
		{
			currentPlayer.setGetOutOfJailFreePotluck(currentCard);
		}
		else
		{ 
			potLuckCards.Enqueue(currentCard);
		}

		// If lose money card
		if (currentCard.GetType() == typeof(LoseMoney))
		{
			LoseMoney card = (LoseMoney)currentCard;

			// Checks if the player will be able to pay the fee on the card
			if (card.amount > currentPlayer.calculateMaxPossibleMoney())
			{
				currentPlayer.Bankrupt();
			}
			else if (card.amount > currentPlayer.GetMoney())
			{
				okButton.interactable = false;
			}
		}
		/*
		else if (currentCard.GetType() == typeof(Birthday))
		{
			Birthday card = (Birthday)currentCard;

			if (card.amount > currentPlayer.calculateMaxPossibleMoney())
			{
				currentPlayer.Bankrupt();
			}
			else if (card.amount > currentPlayer.GetMoney())
			{
				okButton.interactable = false;
			}
		}
		*/

		//board.SetPotluckCards(potLuckCards);

		// Checks if the player went bankrupt from the card
		if (currentPlayer.GetMoney() >= 0)
		{
			potLuckPopup.SetActive(true);
		}
	}
}
