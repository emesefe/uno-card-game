using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    [SerializeField] private SOCard[] soCards;
    [SerializeField] private GameObject cardPrefab;

    private Dictionary<string, Color> cardColors = new Dictionary<string, Color>() 
    {
        {"Red", Color.red},
        {"Green", Color.green},
        {"Blue", Color.blue},
        {"Yellow", Color.yellow}
    };

    [SerializeField] private List<Card> drawDeck;

    private void Start()
    {
        CreateCard(soCards[13], Color.black, 0);
        CreateCard(soCards[9], cardColors["Green"], 1);
    }

    private void CreateCard(SOCard soCard, Color color, int idx)
    {
        GameObject newCard = Instantiate(cardPrefab);
        Card card = newCard.GetComponent<Card>();
        card.SetupCardVisuals(soCard, color);
        card.SetupOrderInLayer(idx);

        drawDeck.Add(card);
    }

}
