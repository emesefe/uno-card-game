using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Player[] players;

    private CardsManager cardsManager;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one instance");
        }

        Instance = this;
    }

    private void Start()
    {
        cardsManager = FindObjectOfType<CardsManager>();
        
        // Inicializar partida
        cardsManager.CreateDrawDeck();
        cardsManager.ShuffleDeck();

        players[0].InitializePlayerHand();
        cardsManager.AddCardToDiscardDeck(cardsManager.DrawCardFromDrawDeck());


        Debug.Log(players[0].CanPlayCard(players[0].GetPlayerHandCards()[0]));
    }

    public CardsManager GetCardsManager()
    {
        return cardsManager;
    }
}
