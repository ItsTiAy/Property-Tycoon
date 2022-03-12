using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Utility : BoardTile
{
	public int cost;

	public override void PerformAction()
	{
		Debug.Log("Utility Space");
		Debug.Log("Cost: " + cost);
	}
}
