using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private int num1;
    private int num2;

    public int RollDice()
    {
        num1 = Random.Range(1, 6);
        num2 = Random.Range(1, 6);
        int total = num1 + num2;

        Debug.Log("Dice roll: " + total);

        return total;
    }

    public bool WasDouble()
    {
        if (num1 == num2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}