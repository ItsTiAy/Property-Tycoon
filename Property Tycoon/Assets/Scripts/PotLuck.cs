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

	public PotLuckCard[] potLuckCards;

	public void setObjects(GameObject potLuckPopup, Button rollDiceButton, TMP_Text description)
    {
		this.potLuckPopup = potLuckPopup;
		this.rollDiceButton = rollDiceButton;
		this.description = description;
    }

	public override void PerformAction(Player currentPlayer)
	{
		// Needs to make method to shuffle cards rather than just picking a random one
		int randNum = Random.Range(0, potLuckCards.Length - 1);
		// Sets the description in the popup
		description.text = potLuckCards[randNum].description;


		potLuckPopup.SetActive(true);
	}
}
