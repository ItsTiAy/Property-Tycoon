using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class BuyableTile : BoardTile
{
    // Have to be public for the Json Reader to work
    public int cost;
    public int[] rent;
    public string group;

    protected Player ownedBy;
    protected bool mortgaged;

    protected GameObject propertyPopup;
	protected TMP_Text propertyNameUI;
	protected TMP_Text rentPricesUI;
	protected Button rollDiceButton;
	protected Button buyButton;

    public void setObjects(GameObject propertyPopup, TMP_Text propertyNameUI, TMP_Text rentPricesUI, Button rollDiceButton, Button buyButton)
    {
        this.propertyPopup = propertyPopup;
        this.propertyNameUI = propertyNameUI;
        this.rentPricesUI = rentPricesUI;
        this.rollDiceButton = rollDiceButton;
        this.buyButton = buyButton;
 
    }

    public int GetCost()
    {
        return cost;
    }

    public void SetOwner(Player player)
    {
        ownedBy = player;
    }

    public bool IsMortgaged()
    {
        return mortgaged;
    }

    public void SetMortgaged(bool isMortgaged)
    {
        mortgaged = isMortgaged;
    }
}
