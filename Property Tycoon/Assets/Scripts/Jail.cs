using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Jail : BoardTile
{
	private Button rollDiceButton;

	public void setObjects(Button rollDiceButton)
	{
		this.rollDiceButton = rollDiceButton;
	}

	public override void PerformAction(Player currentPlayer)
	{
		if(currentPlayer.GetCurrentPosition() != 10)
        {
			currentPlayer.GoToJail();
		}
		rollDiceButton.interactable = true;
	}
}
