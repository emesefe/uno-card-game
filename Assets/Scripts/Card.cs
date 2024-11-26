using UnityEngine;
using System;
using System.Collections.Generic;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Card : MonoBehaviour
{
   private SOCard _soCard;
   private CardColor _color;

   [SerializeField] private SpriteRenderer cardSpriteRenderer;
   [SerializeField] private SpriteRenderer symbolSpriteRenderer;

   [SerializeField] private GameObject backCard;
   [SerializeField] private SpriteRenderer backSpriteRenderer;

   private bool _isFaceDown;
   
   public Action PlayCardEffect;

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

   public void ChangeAlpha(float alpha)
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
        return _color;
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

   public void SetEffect()
   {
        PlayCardEffect = CardEffects.SetEffect(_soCard.type);
   }
}
