using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(BoxCollider2D))]
public class SelectableCard : MonoBehaviour
{
    private Player player;
    private Card card;
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private int originalIndex;

    private bool seletectedCard;

    private float distanceToGoUp = 2.5f;
    private float increaseScaleAmount = 1f;

    private float animationTime = 0.25f;



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

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public void SetCard(Card card)
    {
        this.card = card;
    }

    private void OnMouseDown()
    {
        if (player.CanPlayCard(card) && !seletectedCard)
        {
            if (player.GetTotalSelectedCards() <= 0 || CardsManager.AreTwoCardsEqual(card, player.GetSelectedCard()))
            {
                seletectedCard = true;
                player.AddSelectedCard(card);
                UIManager.Instance.EnableConfirmSelectionButton(true);
            }
        } 

        else if (seletectedCard)
        {
            seletectedCard = false;
            player.RemoveSelectedCard(card);

            if (player.GetTotalSelectedCards() <= 0)
            {
                UIManager.Instance.EnableConfirmSelectionButton(false);
            }
        }
    }

    private void OnMouseEnter()
    {
        transform.DOMove(originalPosition + distanceToGoUp * Vector3.up, animationTime);
        transform.DOScale(originalScale +  increaseScaleAmount * Vector3.one, animationTime);
        card.SetupOrderInLayer(player.GetPlayerHandCards().Count);
    }

    private void OnMouseExit()
    {
        if (!seletectedCard)
        {
            transform.DOMove(originalPosition, animationTime);
            transform.DOScale(originalScale, animationTime);
            card.SetupOrderInLayer(originalIndex);
        }
    }
}
