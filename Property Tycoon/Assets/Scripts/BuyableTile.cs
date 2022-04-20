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
    public int groupAmount;

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
        if(isMortgaged)
        {
            ownedBy.IncreaseMoney(cost / 2);
        }
        else
        {
            ownedBy.DecreaseMoney(cost / 2);
        }
    }

    public Color32 GetGroupColour()
    {
        switch (group)
        {
            case "brown":
                return new Color32(134, 76, 56, 255);
            case "blue":
                return new Color32(172, 220, 240, 255);
            case "purple":
                return new Color32(197, 56, 132, 255);
            case "orange":
                return new Color32(236, 139, 44, 255);
            case "red":
                return new Color32(219, 36, 40, 255);
            case "yellow":
                return new Color32(255, 240, 4, 255);
            case "green":
                return new Color32(19, 168, 87, 255);
            case "deepBlue":
                return new Color32(0, 102, 164, 255);
            case "station":
                return Color.black;
            case "utility":
                return Color.grey;
            default:
                return Color.black;
        }
    }
}
