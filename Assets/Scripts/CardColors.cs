using UnityEngine;
using System.Collections.Generic;

public enum CardColor 
{
    Red,
    Green,
    Blue,
    Yellow,
    Black
}

public static class CardColors
{
    public static readonly Dictionary<CardColor, Color> CardColorsDictionary = new Dictionary<CardColor, Color>() 
    {
        {CardColor.Red, Constants.RED_COLOR},
        {CardColor.Green, Constants.GREEN_COLOR},
        {CardColor.Blue, Constants.BLUE_COLOR},
        {CardColor.Yellow, Constants.YELLOW_COLOR},
        {CardColor.Black, Constants.BLACK_COLOR}
    };
    
    public static CardColor ChooseRandomColor()
    {
        int randomIndex = Random.Range(0, Constants.TOTAL_COLORS);
        return (CardColor)randomIndex;
    }
}
