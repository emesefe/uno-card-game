using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardsManager : MonoBehaviour
{
    [SerializeField] private SOCard[] soCards;
    [SerializeField] private GameObject cardPrefab;

    public enum CardColor 
    {
        Red,
        Green,
        Blue,
        Yellow,
        Black
    }

    private Dictionary<string, Color> cardColors = new Dictionary<string, Color>() 
    {
        {"Red", Color.red},
        {"Green", Color.green},
        {"Blue", Color.blue},
        {"Yellow", Color.yellow},
        {"Black", Color.black}
    };

    [SerializeField] private List<Card> drawDeck;

    private void Start()
    {
        CreateDrawDeck();
        ShuffleDeck(drawDeck);
        DrawCardFromDrawDeck();
    }

    private Card CreateCard(SOCard soCard, Color color, int idx)
    {
        GameObject newCard = Instantiate(cardPrefab);
        Card card = newCard.GetComponent<Card>();
        card.SetupCardVisuals(soCard, color);
        card.SetupOrderInLayer(idx);

        drawDeck.Add(card);

        return card;
    }

    private void CreateDrawDeck()
    {
        int layer = 0;
        
        foreach (SOCard soCard in soCards)
        {
            Card newCard = null;
            if (soCard.type == CardType.Plus4 || soCard.type == CardType.ChangeColor)
            {
                for (int i = 0; i < 4; i++)
                {
                    newCard = CreateCard(soCard, cardColors[CardColor.Black.ToString()], layer);
                    newCard.IsFaceDown(true);   
                    layer++;
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    CardColor color = (CardColor)(i % 4);
                    newCard = CreateCard(soCard, cardColors[color.ToString()], layer);
                    newCard.IsFaceDown(true);   
                    layer++;
                }
            }
        }   
    }

    private void ShuffleDeck(List<Card> deck)
    {
        Card auxCard = null;
        for (int i = 0; i < deck.Count; i++)
        {
            int randomIdx = Random.Range(i, deck.Count);
            auxCard = deck[i];
            deck[i] = deck[randomIdx];
            deck[randomIdx] = auxCard;

            deck[i].SetupOrderInLayer(i);
            deck[randomIdx].SetupOrderInLayer(randomIdx);
        }
    }

    public Card DrawCardFromDrawDeck()
    {
        Card drewCard = drawDeck[drawDeck.Count - 1];
        drawDeck.Remove(drewCard);

        Debug.Log($"he robado: {drewCard.GetCardType()} - {drewCard.GetCardDigit()} de color {drewCard.GetColor()}");

        return drewCard;
    }
}
