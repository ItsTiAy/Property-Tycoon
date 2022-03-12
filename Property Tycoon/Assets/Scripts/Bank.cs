using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour
{
	
	public Bank() {} //Constructor

	public Property[] properties;

	public bool isPropertyAvailableForSale(Property p) {
		return true;
		//checks if the property is owned by the bank
	}

	public void SellProperty(Property prop, Player player) {
		//Takes a property from a player and gives them some money
	}

	public void BuyProperty(Property prop, Player player) {
		//Takes money from a player and gives them a property
	}

	public void TakePlayersProperties(Player player) {
		//Returns all the players properties to the bank in the event that they quit the game
	}

	public void SellHouse(int quantity, Property prop, Player player) {
		//Takes houses from a property and gives the player money for them
	}

	public void BuyHouse(int quantity, Property prop, Player player) {
		//Takes money from the player and puts houses on their property
	}
	
	public bool manageRentTransaction(Player p1, Player p2, int amount) {
		//p1 = player sending money
		//p2 = player recieving money

		// Commented out temporary so game compiles
		//p2.awardMoney(amount);
		//return p1.finePlayer(amount);
		return true;
	}
	
}