#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static event Action<Card> OnCardAddedToMainPlayer;    
    
    [SerializeField] private List<Card> hand = new List<Card>();

    [SerializeField] private int totalInitialCards = 7;

    [SerializeField] private Transform handTransform;

    // [Header("Main Player")]
    // [SerializeField] private float[] handHorizontalLimits;
    // [Range(2, 6)][SerializeField] private int visualCardWidth;
    // private float maxDistanceBetweenCenters = 6;
    //
    // private List<Card> selectedCards = new List<Card>();

    [SerializeField] private bool isMainPlayer; 
    
        
    [Header("Testing")]
    [SerializeField] private bool overrideInitialCards;

    [SerializeField] private InitialCardsSelector initialCardsSelector;

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
        
        if (initialCard != null) drewCard = initialCard; // TODO: Remove in the future, this is only for testing
        
        AddCardToPlayerHand(drewCard);
        
        drewCard.ChangeParent(handTransform);
        
        if (isMainPlayer)
        {
            OnCardAddedToMainPlayer?.Invoke(drewCard);
        }
        else drewCard.HideCard();
    }
    
    public bool CanPlayCard(Card cardToPlay)
    {
        Card lastPlayedCard = CardsManager.Instance.GetLastPlayedCard();

        if (GameManager.Instance.GetTotalCardsToDraw() > 0) 
        {
            if (lastPlayedCard.GetCardType() == CardType.Plus2) return cardToPlay.GetCardType() == CardType.Plus2;
            if (lastPlayedCard.GetCardType() == CardType.Plus4) return cardToPlay.GetCardType() == CardType.Plus4;
        }
        
        // TODO: Faltan casos especiales que dependen de las cartas especiales
        if (cardToPlay.GetCardType() == CardType.Plus4 
            || cardToPlay.GetCardType() == CardType.ChangeColor)
        {
            return true;
        }
        
        if (cardToPlay.GetColor() == GameManager.Instance.GetCurrentColor()) 
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
    
    public CardType? GetFirstCardTypeThatCanBePlayed() 
    {
        foreach (Card card in hand)
        {
            if (CanPlayCard(card)) return card.GetCardType();
        }

        return null;
    }
    
    public Card? GetFirstCardThatCanBePlayed() 
    {
        foreach (Card card in hand)
        {
            if (CanPlayCard(card)) return card;
        }

        return null;
    }

    public List<Card> GetPlayerHandCards()
    {
        return hand;
    }
    
    public bool CheckIfHasWon()
    {
        return hand.Count <= 0;
    }
    
    public bool CheckUNO()
    {
        return hand.Count == 1;
    }
    
    public IEnumerator PlayCard(Card card)
    {
        RemoveCardFromPlayerHand(card);
        
        if (CheckIfHasWon())
        {
            UIManager.Instance.ShowWinPanel();
            UIManager.Instance.HideUNOPanel();
            StopAllCoroutines();
            yield return null;
        }
        
        if (CheckUNO())
        {
            UIManager.Instance.ShowUNOPanel();
            StartCoroutine(GameManager.Instance.UNOTimer());
            
            yield return new WaitUntil(() => GameManager.Instance.GetUNOButtonHasBeenPressed());
            UIManager.Instance.HideUNOPanel();
            
            if (GameManager.Instance.GetWhoHasPressedUnoButton() != this )
            {
                // Robo dos cartas por lento
                for (int i = 0; i < 2; i++)
                {
                    DrawCardToPlayerHand();
                }
            }
            
            GameManager.Instance.SetUNOButtonHasBeenPressed(false, -1);
        }
        
        card.PlayCardEffect();
        CardsManager.Instance.AddCardToDiscardDeck(card);

        if (!card.IsPlus4OrChangeColor())
        {
            GameManager.Instance.ChangeCurrentColor(card.GetColor());
        }
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

    public bool IsMainPlayer()
    {
        return isMainPlayer;
    }

    // private void SetCardAsSelectableCard(Card card)
    // {
    //     SelectableCard selectableCard = card.gameObject.AddComponent<SelectableCard>();
    //     selectableCard.SetMainPlayer(this);
    //     selectableCard.SetCard(card);  
    // }
    //
    // /// <summary>
    // /// This function returns the total distance of the player's hand and the distance between the centers of the cards
    // /// </summary>
    // /// <returns>
    // /// A tuple where the first item is totalDistance and the second item is distanceBetweenCenters
    // /// </returns>
    // private (float, float) GetDistances()
    // {
    //     float totalDistance = handHorizontalLimits[1] - handHorizontalLimits[0];
    //     float freeSpace = totalDistance - (visualCardWidth * hand.Count);
    //
    //     float distanceBetweenCards = freeSpace / (hand.Count - 1);
    //     float distanceBetweenCenters = distanceBetweenCards + visualCardWidth;
    //
    //     if (distanceBetweenCenters > maxDistanceBetweenCenters)
    //     {
    //         distanceBetweenCenters = maxDistanceBetweenCenters;
    //         totalDistance = distanceBetweenCenters * (hand.Count - 1) + visualCardWidth;
    //     }
    //
    //     return (totalDistance, distanceBetweenCenters);
    // }
    //
    // private void ArrangePlayerHandCards()
    // {
    //     (float totalDistance, float distanceBetweenCenters) = GetDistances();
    //
    //     float initialX = -(totalDistance / 2) + visualCardWidth / 2f;
    //     for (int i = 0; i < hand.Count; i++)
    //     {
    //         Card card = hand[i];
    //         
    //         Transform cardTransform = card.gameObject.transform;
    //         cardTransform.localPosition = new Vector3(
    //             initialX + distanceBetweenCenters * i, 0, 0);
    //
    //         card.SetOrderInLayer(i);
    //
    //         SelectableCard selectableCard = card.GetComponent<SelectableCard>();
    //         selectableCard.SetOriginalPosition(cardTransform.position);
    //         selectableCard.SetOriginalScale(cardTransform.localScale);
    //         selectableCard.SetOriginalIndex(i);
    //     }
    // }
    //
    // public Card GetSelectedCard()
    // {
    //     return selectedCards[0];
    // }
    //
    // public void AddSelectedCard(Card selectedCard)
    // {
    //     selectedCards.Add(selectedCard);
    // }
    //
    // public void RemoveSelectedCard(Card selectedCard)
    // {
    //     selectedCards.Remove(selectedCard);
    // }
    //
    // public int GetTotalSelectedCards()
    // {
    //     return selectedCards.Count;
    // }
    //
    // private void ClearSelectedCards()
    // {
    //     selectedCards.Clear();
    // }
    //
    // public IEnumerator PlaySelectedCards()
    // {
    //     if (selectedCards.Count <= 0) yield break;
    //
    //     foreach (Card card in selectedCards)
    //     {
    //         Destroy(card.GetComponent<SelectableCard>());
    //         Destroy(card.GetComponent<BoxCollider2D>());
    //         
    //         StartCoroutine(PlayCard(card));
    //     }
    //     
    //     GameManager.Instance.SetChangingTurns(false);
    //     
    //     yield return new WaitUntil(GameManager.Instance.GetColorHasBeenChanged);
    //     
    //     
    //     
    //     GameManager.Instance.SetColorHasBeenChanged(false);
    //     
    //     GameManager.Instance.ChangeTurn();
    //     
    //     UIManager.Instance.EnableConfirmSelectionButton(false);
    //
    //     ClearSelectedCards();
    //     ArrangePlayerHandCards();
    // }
    //
    // private void EnableSelectableCards(bool enable)
    // {
    //     Debug.Log($"Activo? {enable}");
    //     foreach (Card card in hand)
    //     {
    //         float cardAlpha = enable ? 1f : 0.1f;
    //         card.ChangeAlpha(cardAlpha);
    //         
    //         SelectableCard selectableCard = card.gameObject.GetComponent<SelectableCard>();
    //         selectableCard.UpdateCanSelect(enable);
    //     }
    // }

    #endregion
}
