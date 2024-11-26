using System;
using UnityEngine;

[Serializable]
public class InitialCard
{
    public SOCard soCard;
    public CardColor color;
    public int total;
}

public class InitialCardsSelector : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private InitialCard[] initialCards;
    

    public void InitializeHandWithSelection()
    {
        int cardIdx = 0;
        if (initialCards.Length <= 0) Debug.LogError("InitialCardsSelector initialCards.Length <= 0");
        foreach (InitialCard initialCard in initialCards)
        {
            for (int i = 0; i < initialCard.total; i++)
            {
                Card card = CardsManager.Instance.CreateCard(initialCard.soCard, 
                    initialCard.color, cardIdx);
                player.DrawCardToPlayerHand(card);
                cardIdx++;
            }
        }
    }
}
