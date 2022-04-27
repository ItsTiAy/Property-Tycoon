using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class OpportunityKnocks : BoardTile
{
	private GameObject opportunityKnocksPopup;
	private Button rollDiceButton;
	private TMP_Text description;
	private Button okButton;

	private Card getOutOfJailFree;

	private Queue<Card> opportunityKnocksCards;
	private Card currentCard;

	/// <summary>
	/// Sets Objects, text and buttons to be used by this class
	/// </summary>
	/// <param name="opportunityKnocksPopup">The opportunity knocks popup</param>
	/// <param name="rollDiceButton">The roll dice button</param>
	/// <param name="description">The popup description</param>
	/// <param name="okButton">The popup ok button</param>
	public void setObjects(GameObject opportunityKnocksPopup, Button rollDiceButton, TMP_Text description, Button okButton)
	{
		this.opportunityKnocksPopup = opportunityKnocksPopup;
		this.rollDiceButton = rollDiceButton;
		this.description = description;
		this.okButton = okButton;
	}

	public override void PerformAction(Player currentPlayer)
	{
		opportunityKnocksCards = board.GetOpportunityKnocksCards();
		// Sets the current card
		currentCard = opportunityKnocksCards.Peek();
		// Sets the description in the popup
		description.text = currentCard.description;

		okButton.onClick.RemoveAllListeners();

		if (currentCard.GetType() != typeof(MoveForward)
			&& currentCard.GetType() != typeof(MoveBackward)
			&& currentCard.GetType() != typeof(MoveBackwardAmount)
			)
		{
			// Adds an extra listener to the ok button to make the roll dice button interactable if the card is not a
			// move forward, move backward, or move backwards amount card
			okButton.onClick.AddListener(delegate
			{
				rollDiceButton.interactable = true;
			});
		}

		// When pressing the ok button
		okButton.onClick.AddListener(delegate
		{
			currentCard.performCard(currentPlayer);
			opportunityKnocksCards.Dequeue();
		});

		// Gives the card to the current player if it is a get out of jail free card
		if (currentCard.GetType() == typeof(GetOutOfJailFree))
        {
			currentPlayer.setGetOutOfJailFreeOpportunityKnocks(currentCard);
        }
		else
        {
			opportunityKnocksCards.Enqueue(currentCard);
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
		// If repairs card
		else if (currentCard.GetType() == typeof(Repairs))
		{
			Repairs card = (Repairs)currentCard;

			// Checks if the player will be able to pay the fee on the card
			if (card.GetTotalCost(currentPlayer) > currentPlayer.calculateMaxPossibleMoney())
			{
				currentPlayer.Bankrupt();
			}
			else if (card.GetTotalCost(currentPlayer) > currentPlayer.GetMoney())
			{
				okButton.interactable = false;
			}
		}

		// Checks if the player went bankrupt from the card
        if (currentPlayer.GetMoney() >= 0)
        {
			opportunityKnocksPopup.SetActive(true);
		}

		//board.SetOpportunityKnocksCards(opportunityKnocksCards);
	}
}
