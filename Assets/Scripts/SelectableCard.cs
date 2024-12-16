using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(BoxCollider2D))]
public class SelectableCard : MonoBehaviour
{
    private MainPlayer _mainPlayer;
    private Player _player;
    private Card _card;
    
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private int originalIndex;

    private bool selectedCard;

    private float distanceToGoUp = 2.5f;
    private float increaseScaleAmount = 1f;

    private float animationTime = 0.25f;

    private bool canSelect = true; // TODO: Esto hay que cambiarlo porque el turno inicial es aleatorio

    private void OnMouseDown()
    {
        if (!canSelect) return; 
        
        if (_player.CanPlayCard(_card) && !selectedCard)
        {
            if (_mainPlayer.GetTotalSelectedCards() <= 0 || 
                Card.AreTwoCardsEqual(_card, _mainPlayer.GetSelectedCard()))
            {
                selectedCard = true;
                _mainPlayer.AddSelectedCard(_card);
                UIManager.Instance.EnableConfirmSelectionButton(true);
            }
        } 

        else if (selectedCard)
        {
            selectedCard = false;
            _mainPlayer.RemoveSelectedCard(_card);

            if (_mainPlayer.GetTotalSelectedCards() <= 0)
            {
                UIManager.Instance.EnableConfirmSelectionButton(false);
            }
        }
    }

    private void OnMouseEnter()
    {
        if (!canSelect) return; 
        
        transform.DOMove(originalPosition + distanceToGoUp * Vector3.up, animationTime);
        transform.DOScale(originalScale +  increaseScaleAmount * Vector3.one, animationTime);
        _card.SetOrderInLayer(_player.GetPlayerHandCards().Count);
    }

    private void OnMouseExit()
    {
        if (!canSelect) return;
        
        if (!selectedCard)
        {
            transform.DOMove(originalPosition, animationTime);
            transform.DOScale(originalScale, animationTime);
            _card.SetOrderInLayer(originalIndex);
        }
    }
    
    public void SetOriginalPosition(Vector3 position)
    {
        originalPosition = position;
    }

    public void SetOriginalScale(Vector3 scale)
    {
        originalScale = scale;
    }

    public void SetOriginalIndex(int index)
    {
        originalIndex = index;
    }

    public void SetMainPlayer(MainPlayer mainPlayer, Player player)
    {
        _mainPlayer = mainPlayer;
        _player = player;
    }

    public void SetCard(Card card)
    {
        _card = card;
    }

    public void UpdateCanSelect(bool isSelectionAvailable)
    {
        canSelect = isSelectionAvailable;
    }
}
