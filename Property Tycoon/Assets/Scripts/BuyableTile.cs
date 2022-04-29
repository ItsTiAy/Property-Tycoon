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
    protected GameObject otherPropertyPopup;
	protected TMP_Text propertyNameUI;
	protected TMP_Text rentPricesUI;
    protected TMP_Text otherPropertyText;
    protected Button rollDiceButton;
	protected Button buyButton;
    protected Button okButton;

    /// <summary>
    /// Sets Objects, text and buttons to be used by this class
    /// </summary>
    /// <param name="propertyPopup">The property popup</param>
    /// <param name="propertyNameUI">The property popup name text element</param>
    /// <param name="rentPricesUI"> The property popup rent text element</param>
    /// <param name="rollDiceButton">The roll dice button</param>
    /// <param name="buyButton">The buy property button</param>
    /// <param name="otherPropertyPopup">The popup for landing on another players property</param>
    /// <param name="otherPropertyText">The main text for the popup for landing on another players propert</param>
    /// <param name="okButton">The ok button for the popup for landing on another players propert</param>
    public void setObjects(GameObject propertyPopup, TMP_Text propertyNameUI, TMP_Text rentPricesUI, Button rollDiceButton, Button buyButton, GameObject otherPropertyPopup, TMP_Text otherPropertyText, Button okButton)
    {
        this.propertyPopup = propertyPopup;
        this.propertyNameUI = propertyNameUI;
        this.rentPricesUI = rentPricesUI;
        this.rollDiceButton = rollDiceButton;
        this.buyButton = buyButton;
        this.otherPropertyPopup = otherPropertyPopup;
        this.otherPropertyText = otherPropertyText;
        this.okButton = okButton;
    }

    /// <summary>
    /// Returns the cost of a property
    /// </summary>
    /// <returns>The cost of a property</returns>
    public int GetCost()
    {
        return cost;
    }

    /// <summary>
    /// Sets the owner of a property
    /// </summary>
    /// <param name="player">The player to be set owner</param>
    public void SetOwner(Player player)
    {
        ownedBy = player;
    }

    /// <summary>
    /// Returns whether the property is mortgaged
    /// </summary>
    /// <returns>True if the property is mortgaged, false otherwise</returns>
    public bool IsMortgaged()
    {
        return mortgaged;
    }

    /// <summary>
    /// Sets the property to be mortgaged or unmortgaged
    /// </summary>
    /// <param name="isMortgaged">The value you want to set mortgaged to</param>
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

    /// <summary>
    /// Sets the property to mortgaged or unmortgaged without chaning the players money
    /// </summary>
    /// <param name="mortgaged">The value you want to set mortgaged to</param>
    public void SetMortgagedDirect(bool mortgaged)
    {
        this.mortgaged = mortgaged;
    }

    /// <summary>
    /// Returns the colour of the properties group
    /// </summary>
    /// <returns>The colour of the properties group</returns>
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
