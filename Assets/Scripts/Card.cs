using UnityEngine;
using System;
using DG.Tweening;

public class Card : MonoBehaviour
{
   public Action PlayCardEffect;
   
   private SOCard _soCard;
   private CardColor _color;

   [SerializeField] private SpriteRenderer cardSpriteRenderer;
   [SerializeField] private SpriteRenderer symbolSpriteRenderer;

   [SerializeField] private GameObject backCard;
   [SerializeField] private SpriteRenderer backSpriteRenderer;

   public void SetOrderInLayer(int idx)
   {
        cardSpriteRenderer.sortingOrder = 3 * idx;
        symbolSpriteRenderer.sortingOrder = 3 * idx + 1;
        backSpriteRenderer.sortingOrder = 3 * idx + 2; 
   }
   
   public void SetVisuals(SOCard soCard, CardColor color)
   {
        _soCard = soCard;
        _color = color;

        symbolSpriteRenderer.sprite = soCard.sprite;
        cardSpriteRenderer.color = CardColors.CardColorsDictionary[color];
   }
   
   public void SetEffect()
   {
        PlayCardEffect = CardEffects.SetEffect(_soCard.type);
   }

   
   public void ShowCard()
   {
        gameObject.SetActive(true);
   }

   public void HideCard()
   {
        gameObject.SetActive(false);
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
        return _color;
   }

   
   public void IsFaceDown(bool isFaceDown)
   {
        backCard.SetActive(isFaceDown);
   }

   public bool IsPlus4OrChangeColor()
   {
        return _soCard.IsPlus4OrChangeColor();
   }

   
   public void ChangeParent(Transform newParent)
   {
        transform.SetParent(newParent);
        ChangeLocalPosition(Vector3.zero);
   }

   public void ChangeLocalPosition(Vector3 newPosition)
   {
        transform.localPosition = newPosition;
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

   //TODO: No acaba de funcionar para las cartas negras
   public void ChangeAlpha(float alpha)
   {
        cardSpriteRenderer.DOFade(alpha, 0.5f);
   }
   
   
   public static bool AreTwoCardsEqual(Card card1, Card card2)
   {
        if (card1.GetColor() != card2.GetColor()) return false;

        if (card1.GetCardType() != card2.GetCardType()) return false;
       
        if (card1.GetCardType() != CardType.Number) return true;

        if (card1.GetCardDigit() != card2.GetCardDigit()) return false;

        return true;
   }
}
