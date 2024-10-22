using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private List<Card> hand = new List<Card>();

    private int totalInitialCards = 7;

    [SerializeField] private Transform handTransform;

    [SerializeField] private float[] handHorizontalLimits;
    [Range(2, 6)][SerializeField] private int visualCardWidth;

    private void Start()
    {
        ArrangePlayerHandCards();
    }

    private void AddCardToPlayerHand(Card card)
    {
        hand.Add(card);
    }

    private void RemoveCardFromPlayerHand(Card card)
    {
        hand.Remove(card);
    }

    public void InitializePlayerHand()
    {
        for (int i = 0; i < totalInitialCards; i++)
        {
            Card drewCard =  CardsManager.Instance.DrawCardFromDrawDeck();
            
            drewCard.gameObject.transform.SetParent(handTransform);
            drewCard.gameObject.transform.localPosition = Vector3.zero;

            drewCard.IsFaceDown(false);

            AddCardToPlayerHand(drewCard);
        }
    }

    private void ArrangePlayerHandCards()
    {
        float totalDistance = handHorizontalLimits[1] - handHorizontalLimits[0];

        float freeSpace = totalDistance - (visualCardWidth * hand.Count);

        float distanceBetweenCards = freeSpace / (hand.Count - 1);
        
        float distanceBetweenCenters = distanceBetweenCards + visualCardWidth;

        float initialX = handHorizontalLimits[1] - visualCardWidth / 2;

        for (int i = 0; i < hand.Count; i++)
        {
            Transform cardTransform = hand[i].gameObject.transform;
            cardTransform.localPosition = new Vector3(
            initialX - i * distanceBetweenCenters, 0, 0);
            
            float prop = (float)visualCardWidth / Constants.CARD_WIDTH; 
            cardTransform.localScale = new Vector3(visualCardWidth, prop * cardTransform.localScale.y, cardTransform.localScale.z);
        }
    }

    public bool CanPlayCard(Card cardToPlay)
    {
        Card lastPlayedCard = CardsManager.Instance.GetLastPlayedCard();

        // TODO: ¿Qué pasa cuando me han tirado un +2? No puedo tirar ni un +4 ni un comodín
        // TODO: Faltan casos especiales que dependen de las cartas especiales
        if (cardToPlay.GetCardType() == CardType.Plus4 
        || cardToPlay.GetCardType() == CardType.ChangeColor)
        {
            return true;
        }
        
        if (cardToPlay.GetColor() == lastPlayedCard.GetColor()) 
        {
            return true;
        }

        if (cardToPlay.GetCardType() == lastPlayedCard.GetCardType())
        {        
            if (cardToPlay.GetCardType() != CardType.Number) 
            {
                return true;
            }
            else 
            {
                if (cardToPlay.GetCardDigit() == lastPlayedCard.GetCardDigit())
                {
                    return true;
                }
            }
        }

        return false;
    }

    public List<Card> GetPlayerHandCards()
    {
        return hand;
    }
}
