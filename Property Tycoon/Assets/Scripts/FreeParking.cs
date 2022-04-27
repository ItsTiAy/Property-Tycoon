using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class FreeParking : BoardTile
{
	private Button rollDiceButton;
	private int freeParkingMoney;
	private GameObject freeParkingPopup;
	private TMP_Text freeParkingText;

	/// <summary>
	/// Sets Objects, text and buttons to be used by this class
	/// </summary>
	/// <param name="rollDiceButton">The roll dice button</param>
	/// <param name="freeParkingPopup">The free parking popup</param>
	/// <param name="freeParkingText">The free parking popup description text element</param>
	public void setObjects(Button rollDiceButton, GameObject freeParkingPopup, TMP_Text freeParkingText)
	{
		this.rollDiceButton = rollDiceButton;
		this.freeParkingPopup = freeParkingPopup;
		this.freeParkingText = freeParkingText;
	}

	public override void PerformAction(Player currentPlayer)
	{
		freeParkingText.text = "You earned £" + freeParkingMoney + " from free parking";

		// Current player gets all the money in free parking
		currentPlayer.IncreaseMoney(freeParkingMoney);
		freeParkingMoney = 0;
		freeParkingPopup.SetActive(true);
	}

	/// <summary>
	/// Increases the find money in free parking
	/// </summary>
	/// <param name="amount">The amount you want to increase the free parking pool by</param>
	public void AddFineMoney(int amount)
    {
		freeParkingMoney += amount;
    }
}
