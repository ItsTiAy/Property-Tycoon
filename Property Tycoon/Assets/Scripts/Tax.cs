using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Tax : BoardTile
{
	public int taxAmount;

	private Button rollDiceButton;
	private GameObject taxPopup;
	private TMP_Text taxText;
	private Button taxButton;

	public void setObjects(Button rollDiceButton, GameObject taxPopup, TMP_Text taxText, Button taxButton)
	{
		this.rollDiceButton = rollDiceButton;
		this.taxPopup = taxPopup;
		this.taxText = taxText;
		this.taxButton = taxButton;
	}

	public override void PerformAction(Player currentPlayer)
	{
		Debug.Log(taxAmount);

		taxText.text = "You owe £" + taxAmount + " tax";

		// Player goes bankrupt if they cannot afford the tax
		if (currentPlayer.calculateMaxPossibleMoney() < taxAmount)
		{
			currentPlayer.Bankrupt();
		}
		else if (currentPlayer.GetMoney() < taxAmount)
        {
			taxButton.interactable = false;
			taxPopup.SetActive(true);
		}
		else
        {
			taxPopup.SetActive(true);
        }

		taxButton.onClick.RemoveAllListeners();
		taxButton.onClick.AddListener(delegate
		{
			currentPlayer.DecreaseMoney(taxAmount);
		});

	}
}
