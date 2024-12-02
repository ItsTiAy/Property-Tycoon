using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Jail : BoardTile
{
	private Button rollDiceButton;

	/// <summary>
	/// Sets Objects, text and buttons to be used by this class
	/// </summary>
	/// <param name="rollDiceButton">The roll dice button</param>
	public void setObjects(Button rollDiceButton)
	{
		this.rollDiceButton = rollDiceButton;
	}

	public override void PerformAction(Player currentPlayer)
	{
		// If the player is on the go to jail space
		if(currentPlayer.GetCurrentPosition() != 10)
        {
			currentPlayer.GoToJail();
		}
		rollDiceButton.interactable = true;
	}
}
