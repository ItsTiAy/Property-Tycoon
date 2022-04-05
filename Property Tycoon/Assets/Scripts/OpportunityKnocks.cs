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

	public OpportunityKnocksCard[] opportunityKnocksCards;

	public void setObjects(GameObject opportunityKnocksPopup, Button rollDiceButton, TMP_Text description)
	{
		this.opportunityKnocksPopup = opportunityKnocksPopup;
		this.rollDiceButton = rollDiceButton;
		this.description = description;
	}

	public override void PerformAction(Player currentPlayer)
	{
		// TODO: Needs to make method to shuffle cards rather than just picking a random one
		int randNum = Random.Range(0, opportunityKnocksCards.Length - 1);
		// Sets the description in the popup
		description.text = opportunityKnocksCards[randNum].description;
		opportunityKnocksPopup.SetActive(true);
	}
}
