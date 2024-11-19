using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private List<Card> hand = new List<Card>();

    [SerializeField] private int totalInitialCards = 7;

    [SerializeField] private Transform handTransform;

    [Header("Main Player")]
    [SerializeField] private float[] handHorizontalLimits;
    [Range(2, 6)][SerializeField] private int visualCardWidth;
    private float maxDistanceBetweenCenters = 6;

    private List<Card> selectedCards = new List<Card>();

    private CardsManager cardsManager;

    [SerializeField] private bool isMainPlayer; 
    
        
    [Header("Testing")]
    [SerializeField] private bool overrideInitialCards;

    [SerializeField] private InitialCardsSelector initialCardsSelector;

    private void Start()
    {
        cardsManager = FindObjectOfType<CardsManager>();
    }
    
    private void AddCardToPlayerHand(Card card)
    {
        hand.Add(card);

        if (isMainPlayer) SetCardAsSelectableCard(card);
    }
    
    private void RemoveCardFromPlayerHand(Card card)
    {
        hand.Remove(card);
    }
    
    public void InitializePlayerHand()
    {
        if (!overrideInitialCards) // TODO: Remove in the future, this is only for testing
        {
            for (int i = 0; i < totalInitialCards; i++)
            {
                DrawCardToPlayerHand();
            }
        }
        else // TODO: Remove in the future, this is only for testing
        {
            initialCardsSelector.InitializeHandWithSelection();
        }
    }
    
    public void DrawCardToPlayerHand(Card initialCard = null)
    {
        Card drewCard = CardsManager.Instance.DrawCardFromDrawDeck();
        
        if (initialCard != null) drewCard = initialCard;// TODO: Remove in the future, this is only for testing
        
        AddCardToPlayerHand(drewCard);
        
        drewCard.ChangeParent(handTransform);
        
        if (isMainPlayer)
        {
            Transform cardTransform = drewCard.transform;
            // TODO: Hacer función cambiar escala de la carta
            float prop = (float)visualCardWidth / Constants.CARD_WIDTH;
            cardTransform.localScale = new Vector3(
                visualCardWidth, prop * cardTransform.localScale.y, 1);
            
            drewCard.IsFaceDown(false);
            ArrangePlayerHandCards();
        }
        else drewCard.HideCard();
    }
    
    public bool CanPlayCard(Card cardToPlay)
    {
        Card lastPlayedCard = CardsManager.Instance.GetLastPlayedCard();

        if (GameManager.Instance.GetTotalCardsToDraw() > 0 && lastPlayedCard.GetCardType() == CardType.Plus2)
        {
            return cardToPlay.GetCardType() == CardType.Plus2;
        }
        
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
    
    public bool CanPlayAnyCard() 
    {
        foreach (Card card in hand)
        {
            if (CanPlayCard(card)) return true;
        }

        return false;
    }

    public List<Card> GetPlayerHandCards()
    {
        return hand;
    }
    
    public void PlayCard(Card card)
    {
        card.PlayCardEffect();
        RemoveCardFromPlayerHand(card);  
        cardsManager.AddCardToDiscardDeck(card);
    }
    
    // TODO: Completar esta función para que se devuelvan todas las cartas iguales
    public Card FindCardInHand(CardType cardType)
    {
        foreach (Card card in hand)
        {
            if (card.GetCardType() == cardType)
            {
                return card;
            }
        }

        return null;
    }

    #region MAIN PLAYER

    public bool GetIsMainPlayer()
    {
        return isMainPlayer;
    }

    private void SetCardAsSelectableCard(Card card)
    {
        SelectableCard selectableCard = card.gameObject.AddComponent<SelectableCard>();
        selectableCard.SetPlayer(this);
        selectableCard.SetCard(card);  
    }
    
    /// <summary>
    /// This function returns the total distance of the player's hand and the distance between the centers of the cards
    /// </summary>
    /// <returns>
    /// A tuple where the first item is totalDistance and the second item is distanceBetweenCenters
    /// </returns>
    private (float, float) GetDistances()
    {
        float totalDistance = handHorizontalLimits[1] - handHorizontalLimits[0];
        float freeSpace = totalDistance - (visualCardWidth * hand.Count);

        float distanceBetweenCards = freeSpace / (hand.Count - 1);
        float distanceBetweenCenters = distanceBetweenCards + visualCardWidth;

        if (distanceBetweenCenters > maxDistanceBetweenCenters)
        {
            distanceBetweenCenters = maxDistanceBetweenCenters;
            totalDistance = distanceBetweenCenters * (hand.Count - 1) + visualCardWidth;
        }

        return (totalDistance, distanceBetweenCenters);
    }

    private void ArrangePlayerHandCards()
    {
        (float totalDistance, float distanceBetweenCenters) = GetDistances();

        float initialX = -(totalDistance / 2) + visualCardWidth / 2f;
        for (int i = 0; i < hand.Count; i++)
        {
            Card card = hand[i];
            
            Transform cardTransform = card.gameObject.transform;
            cardTransform.localPosition = new Vector3(
                initialX + distanceBetweenCenters * i, 0, 0);

            card.SetupOrderInLayer(i);

            SelectableCard selectableCard = card.GetComponent<SelectableCard>();
            selectableCard.SetOriginalPosition(cardTransform.position);
            selectableCard.SetOriginalScale(cardTransform.localScale);
            selectableCard.SetOriginalIndex(i);
        }
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

    public void PlaySelectedCards()
    {
        if (selectedCards.Count <= 0) return;
        
        foreach (Card card in selectedCards)
        {
            Destroy(card.GetComponent<SelectableCard>());
            Destroy(card.GetComponent<BoxCollider2D>());

            PlayCard(card);
        }
        
        GameManager.Instance.ChangeTurn();
        
        UIManager.Instance.EnableConfirmSelectionButton(false);

        ClearSelectedCards();
        ArrangePlayerHandCards();
    }

    #endregion
}
