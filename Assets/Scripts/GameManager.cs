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
    }

    public CardsManager GetCardsManager()
    {
        return cardsManager;
    }
}
