using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
   [SerializeField] private SOCard soCard;
   [SerializeField] private Color color;

   [SerializeField] private SpriteRenderer cardSpriteRenderer;
   [SerializeField] private SpriteRenderer symbolSpriteRenderer;

   [SerializeField] private GameObject backCard;

   [SerializeField] private bool _isFaceDown;

   private void Start()
   {    
        IsFaceDown(false);
        SetupCardVisuals();
   }

   private void SetupCardVisuals()
   {
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

   private void IsFaceDown(bool isFaceDown)
   {
        _isFaceDown = isFaceDown;
        backCard.SetActive(isFaceDown);
   }
}
