using UnityEngine;
using System;
using System.Collections.Generic;
using DG.Tweening;

public enum CardColor 
{
     Red,
     Green,
     Blue,
     Yellow,
     Black
}

public class Card : MonoBehaviour
{
   private SOCard _soCard;
   private CardColor _cardColor;
   
   private Dictionary<CardColor, Color> cardColors = new Dictionary<CardColor, Color>() 
   {
        {CardColor.Red, Constants.RED_COLOR},
        {CardColor.Green, Constants.GREEN_COLOR},
        {CardColor.Blue, Constants.BLUE_COLOR},
        {CardColor.Yellow, Constants.YELLOW_COLOR},
        {CardColor.Black, Constants.BLACK_COLOR}
   };

   [SerializeField] private SpriteRenderer cardSpriteRenderer;
   [SerializeField] private SpriteRenderer symbolSpriteRenderer;

   [SerializeField] private GameObject backCard;
   [SerializeField] private SpriteRenderer backSpriteRenderer;

   private bool _isFaceDown;


   public Action PlayCardEffect;

   public void SetupOrderInLayer(int idx)
   {
        cardSpriteRenderer.sortingOrder = 3 * idx;
        symbolSpriteRenderer.sortingOrder = 3 * idx + 1;
        backSpriteRenderer.sortingOrder = 3 * idx + 2; 
   }

   public void SetupCardVisuals(SOCard soCard, CardColor color)
   {
        _soCard = soCard;
        _cardColor = color;

        symbolSpriteRenderer.sprite = soCard.sprite;
        cardSpriteRenderer.color = cardColors[color];
   }

   public void ChangeCardAlpha(float alpha)
   {
        cardSpriteRenderer.DOFade(alpha, 0.5f);
   }

   public void ShowCard()
   {
        gameObject.SetActive(true);
   }

   public void HideCard()
   {
        gameObject.SetActive(false);
   }

   public void IsFaceDown(bool isFaceDown)
   {
        _isFaceDown = isFaceDown;
        backCard.SetActive(isFaceDown);
   }

   public CardType GetCardType()
   {
        return _soCard.type;
   }

   public int GetCardDigit()
   {
        return _soCard.digit;
   }

   public CardColor GetColor()
   {
        return _cardColor;
   }

   public void ChangeParent(Transform newParent)
   {
        transform.SetParent(newParent);
        transform.localPosition = Vector3.zero;
   }

   public void ChangeSize(int newWidth, int newHeight = 0)
   {
        if (newHeight > 0)
        {
             transform.localScale = new Vector3(newWidth, newHeight, 1);
             return;
        }
        
        float prop = newWidth / transform.localScale.x;
        transform.localScale = new Vector3(newWidth, prop * transform.localScale.y, 1);
   }

   public void SetCardEffect()
   {
        PlayCardEffect = _soCard.type switch
        {
             CardType.Number => PlayNumberEffect,
             CardType.Skip => PlaySkipEffect,
             CardType.Invert => PlayInvertEffect,
             CardType.Plus2 => PlayPlus2Effect,
             CardType.Plus4 => PlayPlus4Effect,
             CardType.ChangeColor => PlayChageColorEffect,
             _ => throw new Exception($"Unknown card type {_soCard.type}")
        };
   }
   
   private void PlayNumberEffect() 
   {
        Debug.Log("Juego la carta Número");
   }

   private void PlaySkipEffect() 
   {
        Debug.Log("Juego la carta Skip");
        GameManager.Instance.ChangeTurn();
   }

   private void PlayInvertEffect() 
   {
        Debug.Log("Juego la carta Invert");
        GameManager.Instance.ChangeTurnOrder();
   }

   private void PlayPlus2Effect() 
   {
        Debug.Log("Juego la carta Plus2");
        GameManager.Instance.UpdateTotalCardsToDraw(2);
        UIManager.Instance.ShowTotalCardsToDraw(1f);
   }

   private void PlayPlus4Effect() 
   {
        Debug.Log("Juego la carta Plus4");
        GameManager.Instance.UpdateTotalCardsToDraw(4);
        
        // TODO: Diferenciar si juega el Main Player o si juega otro jugador
        if (GameManager.Instance.IsCurrentPlayingPlayerMainPlayer())
        { 
             UIManager.Instance.ShowChangeColorPanel();
        }
        else
        {
             // TODO: Elegir color aleatorio
             GameManager.Instance.ChangeCurrentColor(CardColor.Blue);
        }
   }

   private void PlayChageColorEffect() 
   {
        Debug.Log("Juego la carta ChangeColor");
        
        // TODO: Diferenciar si juega el Main Player o si juega otro jugador
   }
}
