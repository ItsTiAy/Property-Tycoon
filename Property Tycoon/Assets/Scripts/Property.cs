using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Property : BoardTile
{
	//public int ID;
	//public string propertyName;
	//public string color;
	//public string img;
	//public int price;
	//public int[] rentPrices;

	//public int numHouses;
	//public int housePrice;
	//public Player ownedBy;

	public string group;
	public int cost;
	public int[] rent;
	
	
	public Property()
	{
		//Constructor
	} 

	public void addHouses(int numberOfHouses)
	{
		//Increases houses and changes the currentRent
	}

	public void removeHouses(int numberOfHouses)
	{
		//Increases houses and changes the currentRent
	}
	/*
	public int getRentPrice()
	{
		return rentPrices[numHouses];
	}
	*/
	public override void PerformAction()
    {
		Debug.Log("Property Space");
		Debug.Log("Cost: " + cost);
    }
}