using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private List<Card> hand = new List<Card>();

    private CardsManager cardsManager;

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
            hand[i].gameObject.transform.localPosition = new Vector3(
            initialX - i * distanceBetweenCenters, 0, 0);
            // TODO: Modificar escala de las cartas en función de visualCardWidth
        }
    }
}
