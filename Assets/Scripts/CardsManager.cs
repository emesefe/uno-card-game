using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardsManager : MonoBehaviour
{
    public static CardsManager Instance;

    [SerializeField] private SOCard[] soCards;
    [SerializeField] private GameObject cardPrefab;

    [SerializeField] private Transform drawDeckTransform;
    [SerializeField] private Transform discardDeckTransform;

    [SerializeField] private List<Card> drawDeck;
    [SerializeField] private List<Card> discardDeck;
    
    [SerializeField] private DiscardDeckVisuals discardDeckVisuals;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one instance of CardsManager");
        }

        Instance = this;
    }

    // TODO: When finished with testing, turn this function back to private
    public Card CreateCard(SOCard soCard, CardColor color, int idx)
    {
        GameObject newCard = Instantiate(cardPrefab);

        Card card = newCard.GetComponent<Card>();
        card.ChangeParent(drawDeckTransform);
        card.SetVisuals(soCard, color);
        card.SetEffect();
        card.SetOrderInLayer(idx);
        
        drawDeck.Add(card);

        return card;
    }

    public void CreateDrawDeck()
    {
        int layer = 0;
        
        foreach (SOCard soCard in soCards)
        {
            Card newCard = null;
            if (soCard.type == CardType.Plus4 || soCard.type == CardType.ChangeColor)
            {
                for (int i = 0; i < 4; i++)
                {
                    newCard = CreateCard(soCard, CardColor.Black, layer);
                    newCard.IsFaceDown(true);   
                    layer++;
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    CardColor color = (CardColor)(i % 4);
                    newCard = CreateCard(soCard, color, layer);
                    newCard.IsFaceDown(true);   
                    layer++;
                }
            }
        }   
    }

    public void ShuffleDeck()
    {
        Card auxCard = null;
        for (int i = 0; i < drawDeck.Count; i++)
        {
            int randomIdx = Random.Range(i, drawDeck.Count);
            auxCard = drawDeck[i];
            drawDeck[i] = drawDeck[randomIdx];
            drawDeck[randomIdx] = auxCard;

            drawDeck[i].SetOrderInLayer(i);
            drawDeck[randomIdx].SetOrderInLayer(randomIdx);
        }
    }

    public Card DrawCardFromDrawDeck()
    {
        Card drewCard = drawDeck[drawDeck.Count - 1];
        drawDeck.Remove(drewCard);

        return drewCard;
    }

    public void AddCardToDiscardDeck(Card card)
    {
        discardDeck.Add(card);
        
        card.ChangeParent(discardDeckTransform);
        card.ChangeSize(Constants.CARD_WIDTH, Constants.CARD_HEIGHT);
        card.SetOrderInLayer(discardDeck.Count - 1);

        card.IsFaceDown(false);
        card.ShowCard();

        discardDeckVisuals.ArrangeLastPlayedCards(card);
    }

    public Card GetLastPlayedCard()
    {
        return discardDeck[discardDeck.Count - 1];
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
