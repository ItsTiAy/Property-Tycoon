using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Station : BoardTile
{
    public int cost;
    public int[] rent;

    public override void PerformAction()
    {
        Debug.Log("Station Space");
        Debug.Log("Cost: " + cost);
    }
}