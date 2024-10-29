using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(BoxCollider2D))]
public class SelectableCard : MonoBehaviour
{
    private Player player;
    private Card card;
    private Vector3 originalPosition;

    private bool seletectedCard;

    private void Start()
    {
        originalPosition = transform.position;
    }

    public void SetOriginalPosition(Vector3 originalPosition)
    {
        this.originalPosition = originalPosition;
        Debug.Log(originalPosition);
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
                Debug.Log($"Mi carta selecccionada es {card.GetCardType()}-{card.GetCardDigit()}-{card.GetColor()}");
                
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
        //transform.DOLocalMove(originalPosition + 2 * Vector3.up, 0.25f);
        transform.DOMove(originalPosition + 2 * Vector3.up, 0.25f);
        //TODO: Hacer que la carta se ponga por delante
    }

    private void OnMouseExit()
    {
        if (!seletectedCard)
        {
            //transform.DOLocalMove(originalPosition + 2 * Vector3.up, 0.25f);
            transform.DOMove(originalPosition, 0.25f);
        }
    }
}
