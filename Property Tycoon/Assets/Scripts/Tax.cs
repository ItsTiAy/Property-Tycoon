using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Tax : BoardTile
{
	public int taxAmount;

	private Button rollDiceButton;

	public void setObjects(Button rollDiceButton)
	{
		this.rollDiceButton = rollDiceButton;
	}

	public override void PerformAction(Player currentPlayer)
	{
		Debug.Log(taxAmount);
		currentPlayer.DecreaseMoney(taxAmount);
		rollDiceButton.interactable = true;
	}
}
