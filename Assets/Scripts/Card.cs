using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
   private SOCard _soCard;
   private Color _color;

   [SerializeField] private SpriteRenderer cardSpriteRenderer;
   [SerializeField] private SpriteRenderer symbolSpriteRenderer;

   [SerializeField] private GameObject backCard;
   [SerializeField] private SpriteRenderer backSpriteRenderer;

   private bool _isFaceDown;

   public void SetupOrderInLayer(int idx)
   {
        cardSpriteRenderer.sortingOrder = 3 * idx;
        symbolSpriteRenderer.sortingOrder = 3 * idx + 1;
        backSpriteRenderer.sortingOrder = 3 * idx + 2; 
   }

   public void SetupCardVisuals(SOCard soCard, Color color)
   {
        _soCard = soCard;
        _color = color;

        if (soCard.type == CardType.Plus4 
            || soCard.type == CardType.ChangeColor)
        {
            cardSpriteRenderer.color = Color.black;
        }
        else
        {
            cardSpriteRenderer.color = color;
        }

        symbolSpriteRenderer.sprite = soCard.sprite;
   }

   public void IsFaceDown(bool isFaceDown)
   {
        _isFaceDown = isFaceDown;
        backCard.SetActive(isFaceDown);
   }
}
