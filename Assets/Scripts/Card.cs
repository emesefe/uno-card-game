using UnityEngine;
using System;

public class Card : MonoBehaviour
{
   private SOCard _soCard;
   private Color _color;

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

   public void SetupCardVisuals(SOCard soCard, Color color)
   {
        _soCard = soCard;
        _color = color;

        symbolSpriteRenderer.sprite = soCard.sprite;
        cardSpriteRenderer.color = color;
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

   public Color GetColor()
   {
        return _color;
   }

   public void ChangeParent(Transform newParent)
   {
        transform.SetParent(newParent);
        transform.localPosition = Vector3.zero;
   }

   public void SetCardEffect()
   {
        switch (_soCard.type)
        {
            case CardType.Number:
                 PlayCardEffect = PlayNumberEffect;
                 break;
            case CardType.Skip:
                PlayCardEffect = PlaySkipEffect;
                break;
            case CardType.Invert:
                PlayCardEffect = PlayInvertEffect;
                break;
            case CardType.Plus2:
                PlayCardEffect = PlayPlus2Effect;
                break;
            case CardType.Plus4:
                PlayCardEffect = PlayPlus4Effect;
                break;
            case CardType.ChangeColor:
                PlayCardEffect = PlayChageColorEffect;
                break;
            default:
                PlayCardEffect = null;
                break;
        }
   }
   
   public void PlayNumberEffect() 
   {
        Debug.Log("Juego la carta Número");
   }

   public void PlaySkipEffect() 
   {
        Debug.Log("Juego la carta Skip");
        GameManager.Instance.ChangeTurn();
   }

   public void PlayInvertEffect() 
   {
        Debug.Log("Juego la carta Invert");
        GameManager.Instance.ChangeTurnOrder();
   }

   public void PlayPlus2Effect() 
   {
        Debug.Log("Juego la carta Plus2");
        GameManager.Instance.UpdateTotalCardsToDraw(2);
   }

   public void PlayPlus4Effect() 
   {
        Debug.Log("Juego la carta Plus4");
   }

   public void PlayChageColorEffect() 
   {
        Debug.Log("Juego la carta ChangeColor");
   }
}
