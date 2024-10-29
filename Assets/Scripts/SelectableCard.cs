using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(BoxCollider2D))]
public class SelectableCard : MonoBehaviour
{
    private Player player;
    private Card card;
    private Vector3 originalPosition;
    private int originalIndex;

    private bool seletectedCard;

    private float distanceToGoUp = 2.5f;

    public void SetOriginalPosition(Vector3 position)
    {
        originalPosition = position;
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
            }
        } 

        else if (seletectedCard)
        {
            seletectedCard = false;
            player.RemoveSelectedCard(card);
        }
    }

    private void OnMouseEnter()
    {
        transform.DOMove(originalPosition + distanceToGoUp * Vector3.up, 0.25f);
        card.SetupOrderInLayer(player.GetPlayerHandCards().Count);
    }

    private void OnMouseExit()
    {
        if (!seletectedCard)
        {
            transform.DOMove(originalPosition, 0.25f);
            card.SetupOrderInLayer(originalIndex);
        }
    }
}
