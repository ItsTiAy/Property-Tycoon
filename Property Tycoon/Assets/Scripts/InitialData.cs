using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores the initial data to be used in the game
/// </summary>
public static class InitialData
{
    public static int NumPlayers { get; set; } = 0;
    public static bool IsFullGame { get; set; } = true;
    public static int TimeLimit { get; set; }
    public static List<Color> PlayerColours { get; set; } = new List<Color>(new Color[6]);
}
