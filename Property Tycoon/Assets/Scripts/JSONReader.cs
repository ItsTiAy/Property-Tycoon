using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONReader : MonoBehaviour
{
    public TextAsset BoardData;
    public BoardTile[] boardTiles;
    
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

    public TileList tileList = new TileList();

    // Start is called before the first frame update
    void Start()
    {
        boardTiles = new BoardTile[40];

        tileList = JsonUtility.FromJson<TileList>(BoardData.text);

        // Puts all the types of tile into a single array
        for (int i = 0; i < tileList.station.Length; i++)
        {
            boardTiles[tileList.station[i].position - 1] = tileList.station[i]; // numbers in json file dont start at 0 may change this 
        }
        
        for (int i = 0; i < tileList.utility.Length; i++)
        {
            boardTiles[tileList.utility[i].position - 1] = tileList.utility[i];
        }
        
        for (int i = 0; i < tileList.property.Length; i++)
        {
            boardTiles[tileList.property[i].position - 1] = tileList.property[i];
        }

        for (int i = 0; i < tileList.potLuck.Length; i++)
        {
            boardTiles[tileList.potLuck[i].position - 1] = tileList.potLuck[i];
        }

        for (int i = 0; i < tileList.opportunityKnocks.Length; i++)
        {
            boardTiles[tileList.opportunityKnocks[i].position - 1] = tileList.opportunityKnocks[i];
        }

        for (int i = 0; i < tileList.tax.Length; i++)
        {
            boardTiles[tileList.tax[i].position - 1] = tileList.tax[i];
        }

        for (int i = 0; i < tileList.jail.Length; i++)
        {
            boardTiles[tileList.jail[i].position - 1] = tileList.jail[i];
        }

        boardTiles[tileList.go.position - 1] = tileList.go;
        boardTiles[tileList.freeParking.position - 1] = tileList.freeParking;

        boardTiles[28].PerformAction();
    }
}
