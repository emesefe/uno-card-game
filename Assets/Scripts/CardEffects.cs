using System;
using UnityEngine;

public class CardEffects : MonoBehaviour
{
    public static Action SetEffect(CardType cardType)
    {
        return cardType switch
        {
            CardType.Number => PlayNumberEffect,
            CardType.Skip => PlaySkipEffect,
            CardType.Invert => PlayInvertEffect,
            CardType.Plus2 => PlayPlus2Effect,
            CardType.Plus4 => PlayPlus4Effect,
            CardType.ChangeColor => PlayChageColorEffect,
            _ => throw new Exception($"Unknown card type {cardType}")
        };
    }
    
    private static void PlayNumberEffect() 
    {
        Debug.Log("Juego la carta Número");
    }

    private static void PlaySkipEffect() 
    {
        Debug.Log("Juego la carta Skip");
        GameManager.Instance.SetChangingTurns(true);
        GameManager.Instance.ChangeTurn();
    }

    private static void PlayInvertEffect() 
    {
        Debug.Log("Juego la carta Invert");
        GameManager.Instance.ChangeTurnOrder();
    }

    private static void PlayPlus2Effect() 
    {
        Debug.Log("Juego la carta Plus2");
        GameManager.Instance.UpdateTotalCardsToDraw(2);
        UIManager.Instance.ShowTotalCardsToDraw(1f);
    }

    private static void PlayPlus4Effect() 
    {
        Debug.Log("Juego la carta Plus4");
        GameManager.Instance.UpdateTotalCardsToDraw(4);
        UIManager.Instance.ShowTotalCardsToDraw(1f);
        ChangeColor();
    }

    private static void PlayChageColorEffect() 
    {
        Debug.Log("Juego la carta ChangeColor");
        ChangeColor();
    }

    private static void ChangeColor()
    {
        if (GameManager.Instance.IsCurrentPlayerMainPlayer())
        { 
            UIManager.Instance.ShowChangeColorPanel();
        }
        else
        {
            CardColor randomColor = CardColors.ChooseRandomColor();
            GameManager.Instance.ChangeCurrentColor(randomColor);
            UIManager.Instance.ShowCurrentColorText(randomColor, 1);
        }
    }
}
