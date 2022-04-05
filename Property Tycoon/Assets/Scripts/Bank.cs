using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;

public static class Bank
{

	public static List<BuyableTile> properties;

	/*
	public static bool sellProperty(Property prop, Player player)
	{ //tested
	  //need to check if it has any houses on it probably
		if (prop.ownedBy != player)
		{
			Debug.Log("Error: " + player.name + " attempting to sell property they dont own " + prop.name);
			return false;
		}
		player.awardMoney(Decimal.ToInt32(prop.GetCost() / 2));
		player.properties.Remove(prop);
		prop.setOwnershipTo(null);
		properties.Add(prop);
		return true;
	}

	public static bool buyProperty(Property prop, Player player)
	{ //tested
	  //Takes money from a player and gives them a property
		if (!properties.Contains(prop))
		{
			Debug.Log("Error 23546: Property being bought isn't the banks : " + prop.name);
		}
		else if (player.money >= prop.price)
		{
			awardProperty(prop, player);
			properties.Remove(prop);
			player.finePlayer(prop.price);
			return true;
		}
		return false;
	}

	private static void awardProperty(Property property, Player player)
	{ //tested
		property.setOwnershipTo(player);
		player.awardProperty(property);
	}
	*/
	public static void takePlayersProperties(Player player)
	{ //tested
	  //Returns all the players properties to the bank in the event that they quit the game
		foreach (BuyableTile p in player.properties) 
		{ 
			p.SetOwner(null); 
		}
		//foreach (Property p in player.mortgaged) { p.SetOwner(null); }
		properties.AddRange(player.properties); //leaves the properties in the players property list in case you want to show what they ended with?
		//properties.AddRange(player.mortgaged); //leaves the properties in the players property list in case you want to show what they ended with?
	}
	/*
	public static bool sellHouses(Player player, Property h1, int a1, Property h2, int a2, Property h3 = null, int a3 = 0)
	{ //tested
		if (h3 == null)
		{
			if (Property.removeHousesGroup(h1, a1, h2, a2))
			{
				player.money += (a1 + a2) * h1.housePrice; //change amount given back
				return true;
			}
			else { return false; }
		}
		else
		{
			if (Property.removeHousesGroup(h1, a1, h2, a2, h3, a3))
			{
				player.money += (a1 + a2 + a3) * h1.housePrice;
				return true;
			}
			else { return false; }
		}
	}

	public static bool buyHouses(Player player, Property h1, int a1, Property h2, int a2, Property h3 = null, int a3 = 0)
	{ //tested
	  //Takes money from the player and puts houses on their property
		if ((a1 + a2 + a3) * h1.housePrice <= player.money)
		{ //if the player can afford the houses
			if (h3 == null)
			{
				if (Property.addHousesGroup(h1, a1, h2, a2))
				{
					player.money -= (a1 + a2) * h1.housePrice;
					return true;
				}
				else { return false; }
			}
			else
			{
				if (Property.addHousesGroup(h1, a1, h2, a2, h3, a3))
				{
					player.money -= (a1 + a2 + a3) * h1.housePrice;
					return true;
				}
				else { return false; }
			}
		}
		else
		{
			Debug.Log("Error 4536: Not enough money to buy properties: " + ((a1 + a2 + a3) * h1.housePrice) + " + " + player.money);
			return false;
		}
	}
	
	public static bool manageRentTransaction(Player p1, Player p2, int amount)
	{
		//p1 = player sending money
		//p2 = player recieving money
		p2.awardMoney(amount);
		return p1.finePlayer(amount);
	}
	*/
	public static bool setProperties()
	{
		//loads in text file
		return false;
	}
}