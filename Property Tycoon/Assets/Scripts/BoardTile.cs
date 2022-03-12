using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardTile
{
	
	//public int ID;
	
	//public Property property;
	//public int type;
	//public int taxPrice;

	public int position;
	public string tileName;
	public bool buyable;

	/*
	public bool PerformAction(Player player) {
		if (type == 0) { //property
			return performActionProperty(player);
		} else if (type == 1) { //tax
			return true; //player.finePlayer(taxPrice); temporary
		} else if (type == 2) { //community chest
			
		} else if (type == 3) { //chance
		
		} else if (type == 4) { //free parking
		
		} else if (type == 5) { //go to jail
		
		} else if (type == 6) { //nothing (go/visiting jail)
		}
		return true; //the player is still in the game
	}
	
	private bool sellProperty() {
		/*
		if (player.money >= property.price) {
			//check if player wants to buy it
			if (true == true) { //if the player wants to buy it
				player.purchase(property);
				return true; //the player has bought the property
			}
		}
		 // temporary comment out
		return false; //player didn't buy it
	}
	
	private void auctionProperty() {
		//make bidding system
	}
	
	private bool performActionCard() {
		return true;
		
	}
	
	private bool performActionProperty(Player player) {
		if (property.ownedBy == player) {
			//do nothing
		} else if (property.ownedBy == null) {
			if (!sellProperty()) { //if the property isn't sold
				auctionProperty();
			}
		} else { //if property is owned by another player
			//return bank.manageRentTransaction(player, property.ownedBy, property.getRentPrice()); //returns false if player can't pay
			return false; // temporary
		}
		return true;
	}
*/
	public virtual void PerformAction()
	{
		// Wack innit	
	}
}