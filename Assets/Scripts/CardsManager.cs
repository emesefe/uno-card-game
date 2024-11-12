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
        {"Red", Constants.RED_COLOR},
        {"Green", Constants.GREEN_COLOR},
        {"Blue", Constants.BLUE_COLOR},
        {"Yellow", Constants.YELLOW_COLOR},
        {"Black", Constants.BLACK_COLOR}
    };

    [SerializeField] private List<Card> drawDeck;
    [SerializeField] private List<Card> discardDeck;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one instance of CardsManager");
        }

        Instance = this;
    }

    // TODO: When finished with testing, turn this function back to private
    public Card CreateCard(SOCard soCard, Color color, int idx)
    {
        GameObject newCard = Instantiate(cardPrefab);

        Card card = newCard.GetComponent<Card>();
        card.ChangeParent(drawDeckTransform);
        card.SetupCardVisuals(soCard, color);
        card.SetCardEffect();
        card.SetupOrderInLayer(idx);
        
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
                    newCard = CreateCard(soCard, GetColorFromCardColor(CardColor.Black), layer);
                    newCard.IsFaceDown(true);   
                    layer++;
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    CardColor color = (CardColor)(i % 4);
                    newCard = CreateCard(soCard, GetColorFromCardColor(color), layer);
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

            drawDeck[i].SetupOrderInLayer(i);
            drawDeck[randomIdx].SetupOrderInLayer(randomIdx);
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
        
        // TODO: Hacer función cambiar escala de la carta
        card.gameObject.transform.localScale = new Vector3(Constants.CARD_WIDTH, Constants.CARD_HEIGHT, 1);

        card.SetupOrderInLayer(discardDeck.Count - 1);

        card.IsFaceDown(false);
        card.ShowCard();
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

    public Color GetColorFromCardColor(CardColor cardColor)
    {
        return cardColors[cardColor.ToString()];
    }
}
