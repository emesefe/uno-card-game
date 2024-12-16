using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayer : MonoBehaviour
{
    [SerializeField] private Player player;
    
    [SerializeField] private float[] handHorizontalLimits;
    [Range(2, 6)][SerializeField] private int visualCardWidth;
    private float maxDistanceBetweenCenters = 6;

    private List<Card> selectedCards = new List<Card>();
    
    private void OnEnable()
    {
        GameManager.OnTurnChanged += GameManager_OnTurnChanged;
        Player.OnCardAddedToMainPlayer += Player_OnCardAddedToMainPlayer;
    }

    private void OnDisable()
    {
        GameManager.OnTurnChanged -= GameManager_OnTurnChanged;
        Player.OnCardAddedToMainPlayer -= Player_OnCardAddedToMainPlayer;
    }
    
    private void GameManager_OnTurnChanged(Turn currentTurn)
    {
        Player currentPlayingPlayer = GameManager.Instance.GetCurrentPlayer();
        bool isCurrentPlayingPlayer = currentPlayingPlayer == player;
        
        EnableSelectableCards(isCurrentPlayingPlayer);
    }
    
    private void Player_OnCardAddedToMainPlayer(Card card)
    {
        AddCard(card);
    }

    private void AddCard(Card card)
    {
        SetCardAsSelectableCard(card);
        card.ChangeSize(visualCardWidth);
        card.IsFaceDown(false);
            
        ArrangePlayerHandCards();
    }
    
    private void SetCardAsSelectableCard(Card card)
    {
        SelectableCard selectableCard = card.gameObject.AddComponent<SelectableCard>();
        selectableCard.SetMainPlayer(this, player);
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
        List<Card> hand = player.GetPlayerHandCards();
        
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
        List<Card> hand = player.GetPlayerHandCards();
        (float totalDistance, float distanceBetweenCenters) = GetDistances();

        float initialX = -(totalDistance / 2) + visualCardWidth / 2f;
        for (int i = 0; i < hand.Count; i++)
        {
            Card card = hand[i];
            
            Transform cardTransform = card.gameObject.transform;
            cardTransform.localPosition = new Vector3(
                initialX + distanceBetweenCenters * i, 0, 0);

            card.SetOrderInLayer(i);

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

    public IEnumerator PlaySelectedCards()
    {
        if (selectedCards.Count <= 0) yield break;

        foreach (Card card in selectedCards)
        {
            Destroy(card.GetComponent<SelectableCard>());
            Destroy(card.GetComponent<BoxCollider2D>());
            
            StartCoroutine(player.PlayCard(card));
        }
        
        GameManager.Instance.SetChangingTurns(false);
        
        yield return new WaitUntil(GameManager.Instance.GetColorHasBeenChanged);
        
        GameManager.Instance.SetColorHasBeenChanged(false);
        
        GameManager.Instance.ChangeTurn();
        
        UIManager.Instance.EnableConfirmSelectionButton(false);

        ClearSelectedCards();
        ArrangePlayerHandCards();
    }

    private void EnableSelectableCards(bool enable)
    {
        List<Card> hand = player.GetPlayerHandCards();
        
        foreach (Card card in hand)
        {
            float cardAlpha = enable ? 1f : 0.1f;
            card.ChangeAlpha(cardAlpha);
            
            SelectableCard selectableCard = card.gameObject.GetComponent<SelectableCard>();
            selectableCard.UpdateCanSelect(enable);
        }
    }
}
