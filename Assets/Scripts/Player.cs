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
        if (!overrideInitialCards) // This is the game's logic
        {
            for (int i = 0; i < totalInitialCards; i++)
            {
                DrawCardToPlayerHand();
            }
        }
        else // This is for testing
        {
            initialCardsSelector.InitializeHandWithSelection();
        }
    }
    
    public void DrawCardToPlayerHand(Card initialCard = null)
    {
        Card drewCard = CardsManager.Instance.DrawCardFromDrawDeck();
        
        if (initialCard != null) drewCard = initialCard; // This is for testing
        
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
        
        if (cardToPlay.GetCardType() == CardType.Plus4 || 
            cardToPlay.GetCardType() == CardType.ChangeColor)
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
            
            if (cardToPlay.GetCardDigit() == lastPlayedCard.GetCardDigit())
            {
                return true;
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
        return GetFirstCardThatCanBePlayed()?.GetCardType();
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
    
    private bool CheckIfHasWon()
    {
        return hand.Count <= 0;
    }
    
    private bool CheckUNO()
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

    public bool IsMainPlayer()
    {
        return isMainPlayer;
    }
}
