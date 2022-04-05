using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class FreeParking : BoardTile
{
	private Button rollDiceButton;
	private int freeParkingMoney;

	public void setObjects(Button rollDiceButton)
	{
		this.rollDiceButton = rollDiceButton;
	}

	public override void PerformAction(Player currentPlayer)
	{
		Debug.Log("Player earned " + freeParkingMoney + " from free parking");
		currentPlayer.IncreaseMoney(freeParkingMoney);
		freeParkingMoney = 0;
		rollDiceButton.interactable = true;
	}

	public void AddFineMoney(int amount)
    {
		freeParkingMoney += amount;
    }
}
