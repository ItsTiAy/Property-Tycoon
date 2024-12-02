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

	/// <summary>
	/// Sets the properties buyable value
	/// </summary>
	/// <param name="buyable">The value that you want to set buyable to</param>
	public void SetBuyable(bool buyable)
    {
		this.buyable = buyable;
    }

	public string getName()
    {
		return tileName;
    }

	/// <summary>
	/// Sets the board so certain cards can access methods
	/// </summary>
	/// <param name="board">The board to be set</param>
	public void setBoard(Board board)
    {
		this.board = board;
    }

	/// <summary>
	/// Action performed when the tile is landed on
	/// </summary>
	/// <param name="currentPlayer">The current player</param>
	public virtual void PerformAction(Player currentPlayer)
	{
		// Wack innit	
	}
}