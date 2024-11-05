using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Turn {
        Player01,
        Player02,
        Player03,
        Player04,
        Player05,
        Player06,
        Player07,
        Player08,
        Player09,
        Player10,
    }

    public static GameManager Instance;

    [SerializeField] private Player[] players;
    private int totalPlayingPlayers = 2;

    private Turn currentTurn;
    private bool turnOrderClockwise;

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
        currentTurn = Turn.Player01;

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

    public void ChangeTurn(int turnsToChange = 1)
    {
        int currentTurnIdx = (int)currentTurn; 
    
        if (turnOrderClockwise)
        {
            currentTurnIdx += turnsToChange;
            if (currentTurnIdx >= totalPlayingPlayers) 
            {
                currentTurnIdx = currentTurnIdx - totalPlayingPlayers;
            }
        }
        else 
        {
            currentTurnIdx -= turnsToChange;
            if (currentTurnIdx < 0) 
            {
                currentTurnIdx = currentTurnIdx + totalPlayingPlayers;
            }
        }
        
        currentTurn = (Turn)currentTurnIdx;
    }
}
