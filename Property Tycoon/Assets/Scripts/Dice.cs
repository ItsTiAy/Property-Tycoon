using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores infomation and methods for the dice
/// </summary>
public class Dice : MonoBehaviour
{
    private int num1;
    private int num2;

    /// <summary>
    /// Rolls the dice
    /// </summary>
    /// <returns>The total value of the dice</returns>
    public int RollDice()
    {
        num1 = Random.Range(1, 6);
        num2 = Random.Range(1, 6);
        
        //num1 = 0;
        //num2 = 12;
        int total = num1 + num2;

        Debug.Log("Dice roll: " + total);

        return total;
    }

    /// <summary>
    /// Returns the value of dice1
    /// </summary>
    /// <returns>The value of dice1</returns>
    public int GetDice1()
    {
        return num1;
    }

    /// <summary>
    /// Returns the value of dice2
    /// </summary>
    /// <returns>The value of dice2</returns>
    public int GetDice2()
    {
        return num2;
    }

    /// <summary>
    /// Returns whether the last roll was a double
    /// </summary>
    /// <returns>True if the last roll was a double, false if not</returns>
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