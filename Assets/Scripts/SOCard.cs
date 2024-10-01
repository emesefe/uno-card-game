using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType 
{
    Number,
    Skip,
    Invert,
    Plus2,
    Plus4,
    ChangeColor
}

[CreateAssetMenu(menuName = "Card", fileName = "New Card")]
public class SOCard : ScriptableObject
{
    public CardType type;
    public int digit;
    public Sprite sprite;
}
