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

	public void setObjects(GameObject opportunityKnocksPopup, Button rollDiceButton, TMP_Text description, Button okButton)
	{
		this.opportunityKnocksPopup = opportunityKnocksPopup;
		this.rollDiceButton = rollDiceButton;
		this.description = description;
		this.okButton = okButton;
	}

	public override void PerformAction(Player currentPlayer)
	{
		// TODO: Needs to make method to shuffle cards rather than just picking a random one
		//int randNum = Random.Range(0, opportunityKnocksCards.Count - 1);
		// Sets the description in the popup
		opportunityKnocksCards = board.GetOpportunityKnocksCards();
		currentCard = opportunityKnocksCards.Dequeue();
		description.text = currentCard.description;

		okButton.onClick.RemoveAllListeners();
		okButton.onClick.AddListener(delegate
		{
			currentCard.performCard(currentPlayer);
		});

		opportunityKnocksCards.Enqueue(currentCard); // Unless get out of jail free card
		board.SetOpportunityKnocksCards(opportunityKnocksCards);

		opportunityKnocksPopup.SetActive(true);
	}
}
