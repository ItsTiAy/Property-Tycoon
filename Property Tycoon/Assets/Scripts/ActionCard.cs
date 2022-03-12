using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCard : MonoBehaviour
{

	public int type; //fine, get out of jail, housing fees, move to, etc.
	public int ID;

	public ActionCard() {} //Constructor
	
	public void performCardAction(Player player) {
		//dependant on the type, calls the relevant method
	}

	private void finePlayer(Player player) {
		//player.finePlayer(amount)
	}
}