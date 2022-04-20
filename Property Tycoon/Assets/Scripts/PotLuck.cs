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

	public Queue<Card> potLuckCards;

	private Card currentCard;

	public void setObjects(GameObject potLuckPopup, Button rollDiceButton, TMP_Text description, Button okButton)
    {
		this.potLuckPopup = potLuckPopup;
		this.rollDiceButton = rollDiceButton;
		this.description = description;
		this.okButton = okButton;
    }

	public override void PerformAction(Player currentPlayer)
	{
		// Needs to make method to shuffle cards rather than just picking a random one
		//int randNum = Random.Range(0, potLuckCards.Count - 1);
		// Sets the description in the popup
		//description.text = potLuckCards[randNum].description;

		//potLuckCards[randNum].performCard(currentPlayer);
		potLuckCards = board.GetPotluckCards();
		currentCard = potLuckCards.Dequeue();
		description.text = currentCard.description;

		okButton.onClick.RemoveAllListeners();
		okButton.onClick.AddListener(delegate
		{
			currentCard.performCard(currentPlayer);
		});

		potLuckCards.Enqueue(currentCard); // Unless get out of jail free card
		board.SetPotluckCards(potLuckCards);

		potLuckPopup.SetActive(true);
	}
}
