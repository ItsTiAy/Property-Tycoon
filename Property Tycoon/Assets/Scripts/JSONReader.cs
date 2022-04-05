using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JSONReader : MonoBehaviour
{
    public TextAsset BoardData;
    public TextAsset CardData;

    public BoardTile[] boardTiles;

    public GameObject potLuckPopup;
    public GameObject opportunityKnocksPopup;
    public GameObject propertyPopup;
    public TMP_Text propertyNameUI;
    public TMP_Text rentPricesUI;
    public TMP_Text potLuckDescription;
    public TMP_Text opportunityKnocksDescription;
    public Button rollDiceButton;
    public Button buyButton;

    // Class for storing the data for all the tiles
    [System.Serializable]
    public class TileList
    {
        public Property[] property;
        public Utility[] utility;
        public Station[] station;
        public PotLuck[] potLuck;
        public OpportunityKnocks[] opportunityKnocks;
        public Tax[] tax;
        public Jail[] jail;
        public Go go;
        public FreeParking freeParking;
    }

    // Class for storing the card data
    [System.Serializable]
    public class Card
    {
        public PotLuckCard[] potLuckCards;
        public OpportunityKnocksCard[] opportunityKnocksCards;
    }

    public TileList tileList = new TileList();
    public Card card = new Card();

    public BoardTile[] GetBoardTiles()
    {
        boardTiles = new BoardTile[40];

        // Reads data from the JSON file BoardData and puts it into each array in TileList
        tileList = JsonUtility.FromJson<TileList>(BoardData.text);
        // Reads data from the JSON file CardData and puts it into each array in Card
        card = JsonUtility.FromJson<Card>(CardData.text);
        
        // Puts all the types of tile into a single array

        for (int i = 0; i < tileList.property.Length; i++) // Property
        {
            // Sets the UI objects needed to be accessed from this class (Same for others underneath)
            tileList.property[i].setObjects(propertyPopup, propertyNameUI, rentPricesUI, rollDiceButton, buyButton);
            // Adds the object into the boardTiles array at the position it is on the board (Same for others underneath)
            boardTiles[tileList.property[i].position - 1] = tileList.property[i];
        }

        for (int i = 0; i < tileList.station.Length; i++) // Station
        {
            tileList.station[i].setObjects(propertyPopup, propertyNameUI, rentPricesUI, rollDiceButton, buyButton);
            boardTiles[tileList.station[i].position - 1] = tileList.station[i]; // numbers in json file dont start at 0 may change this 
        }
        
        for (int i = 0; i < tileList.utility.Length; i++) // Utility
        {
            tileList.utility[i].setObjects(propertyPopup, propertyNameUI, rentPricesUI, rollDiceButton, buyButton);
            boardTiles[tileList.utility[i].position - 1] = tileList.utility[i];
        }

        for (int i = 0; i < tileList.potLuck.Length; i++) // PotLuck
        {
            // Adds all the PotLuck cards into the array in the PotLuck class
            tileList.potLuck[i].potLuckCards = card.potLuckCards;
            tileList.potLuck[i].setObjects(potLuckPopup, rollDiceButton, potLuckDescription);
            boardTiles[tileList.potLuck[i].position - 1] = tileList.potLuck[i];
        }

        for (int i = 0; i < tileList.opportunityKnocks.Length; i++) // OpportunityKnocks
        {
            // Adds all the OpportunityKnocks cards into the array in the OpportunityKnocks class
            tileList.opportunityKnocks[i].opportunityKnocksCards = card.opportunityKnocksCards;
            tileList.opportunityKnocks[i].setObjects(opportunityKnocksPopup, rollDiceButton, opportunityKnocksDescription);
            boardTiles[tileList.opportunityKnocks[i].position - 1] = tileList.opportunityKnocks[i];
        }

        for (int i = 0; i < tileList.tax.Length; i++) // Tax
        {
            tileList.tax[i].setObjects(rollDiceButton);
            boardTiles[tileList.tax[i].position - 1] = tileList.tax[i];
        }

        for (int i = 0; i < tileList.jail.Length; i++) // Jail
        {
            tileList.jail[i].setObjects(rollDiceButton);
            boardTiles[tileList.jail[i].position - 1] = tileList.jail[i];
        }

        // Go
        tileList.go.setObjects(rollDiceButton);
        boardTiles[tileList.go.position - 1] = tileList.go;

        // Free parking
        tileList.freeParking.setObjects(rollDiceButton);
        boardTiles[tileList.freeParking.position - 1] = tileList.freeParking;
        
        return boardTiles;
    }
}
