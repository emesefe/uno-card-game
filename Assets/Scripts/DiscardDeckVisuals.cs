using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DiscardDeckVisuals : MonoBehaviour
{
    private Vector3[] lastDiscardedCardsPositions = new Vector3[]
    {
        new Vector3(-9, 0, 0),
        new Vector3(-6, 0, 0),
        new Vector3(-3, 0, 0),
        new Vector3(0, 0, 0)
    };
    
    private List<Card> lastDiscardedCards = new List<Card>();

    private int totalDiscardedCardsToShow = 4;

    private void Start()
    {
        if (lastDiscardedCardsPositions.Length != totalDiscardedCardsToShow)
        {
            Debug.LogError($"lastDiscardedCardsPositions must have {totalDiscardedCardsToShow} Vector3 elements");
        }
    }

    public void ArrangeLastPlayedCards(Card card)
    {
        AddNewPlayedCard(card);

        int offset = totalDiscardedCardsToShow - lastDiscardedCards.Count;
        for (int i = 0; i < lastDiscardedCards.Count; i++)
        {
            Card discardedCard = lastDiscardedCards[i];
            Vector3 position = lastDiscardedCardsPositions[i + offset];
            
            discardedCard.ChangeLocalPosition(position);
        }
    }

    private void AddNewPlayedCard(Card card)
    {
        if (lastDiscardedCards.Count >= totalDiscardedCardsToShow)
        {
            // We delete the first card to always have less or equal than totalDiscardedCardsToShow
            lastDiscardedCards.RemoveAt(0); 
        }
        
        lastDiscardedCards.Add(card);
    }
}
