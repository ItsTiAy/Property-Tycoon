using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardTile
{
	public int position;
	public string tileName;
	public bool buyable;

	protected Board board;

	public void SetBuyable(bool buyable)
    {
		this.buyable = buyable;
    }

	public string getName()
    {
		return tileName;
    }

	public void setBoard(Board board)
    {
		this.board = board;
    }

	public virtual void PerformAction(Player currentPlayer)
	{
		// Wack innit	
	}
}