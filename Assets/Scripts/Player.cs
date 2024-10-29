using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private List<Card> hand = new List<Card>();

    [SerializeField] private int totalInitialCards = 7;

    [SerializeField] private Transform handTransform;

    [SerializeField] private float[] handHorizontalLimits;
    [Range(2, 6)][SerializeField] private int visualCardWidth;
    private float maxDistanceBetweenCenters = 6;

    private List<Card> selectedCards = new List<Card>();

    private CardsManager cardsManager;

    private void Start()
    {
        cardsManager = FindObjectOfType<CardsManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            PlaySelectedCards();
        }
    }

    private void AddCardToPlayerHand(Card card)
    {
        hand.Add(card);

        //TODO: Averiguar si es el player principal (bool)
        
        SelectableCard selectableCard = card.gameObject.AddComponent<SelectableCard>();
        selectableCard.SetPlayer(this);
        selectableCard.SetCard(card);
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
            
            float prop = (float)visualCardWidth / Constants.CARD_WIDTH; 
            Transform cardTransform = drewCard.transform;
            cardTransform.localScale = new Vector3(visualCardWidth, prop * cardTransform.localScale.y, cardTransform.localScale.z);

            drewCard.IsFaceDown(false);

            AddCardToPlayerHand(drewCard);
        }

        ArrangePlayerHandCards();
    }

    private void ArrangePlayerHandCards()
    {
        float totalDistance = handHorizontalLimits[1] - handHorizontalLimits[0];

        float freeSpace = totalDistance - (visualCardWidth * hand.Count);

        float distanceBetweenCards = freeSpace / (hand.Count - 1);
        
        float distanceBetweenCenters = distanceBetweenCards + visualCardWidth;
        
        distanceBetweenCenters = distanceBetweenCenters > maxDistanceBetweenCenters 
        ? maxDistanceBetweenCenters
        : distanceBetweenCenters;

        float initialX = hand.Count % 2 == 0 ?  distanceBetweenCenters / 2 : 0;

        for (int i = 0; i < hand.Count; i++)
        {
            Card card = hand[i];
            Transform cardTransform = card.gameObject.transform;
            cardTransform.localPosition = new Vector3(
                initialX + Mathf.Pow(-1, i) * (i / 2 + i % 2) * distanceBetweenCenters, 0, 0);
            card.GetComponent<SelectableCard>().SetOriginalPosition(cardTransform.localPosition);
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

    public Card GetSelectedCard()
    {
        return selectedCards[0];
    }

    public void AddSelectedCard(Card selectedCard)
    {
        selectedCards.Add(selectedCard);
    }

    public void RemoveSelectedCard(Card selectedCard)
    {
       selectedCards.Remove(selectedCard);
    }

    public int GetTotalSelectedCards()
    {
        return selectedCards.Count;
    }

    private void ClearSelectedCards()
    {
        selectedCards.Clear();
    }

    private void PlaySelectedCards()
    {
        if (selectedCards.Count <= 0) return;
        
        foreach (Card card in selectedCards)
        {
            Destroy(card.GetComponent<SelectableCard>());
            Destroy(card.GetComponent<BoxCollider2D>());

            RemoveCardFromPlayerHand(card);  
            cardsManager.AddCardToDiscardDeck(card);
        }

        ClearSelectedCards();
        ArrangePlayerHandCards();
    }
}
