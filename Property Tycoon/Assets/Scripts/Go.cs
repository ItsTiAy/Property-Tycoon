using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Go : BoardTile
{
	private Button rollDiceButton;

	public void setObjects(Button rollDiceButton)
	{
		this.rollDiceButton = rollDiceButton;
	}

	public override void PerformAction(Player currentPlayer)
	{
		rollDiceButton.interactable = true;
	}
}
