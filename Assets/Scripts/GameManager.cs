using UnityEngine;
using System.Collections;

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

    private int totalCardsToDraw;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one instance of GameManager");
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
        
        turnOrderClockwise = true;
        totalCardsToDraw = 0;

        foreach (Player player in players)
        {
            player.InitializePlayerHand();
        }
        
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
                currentTurnIdx -= totalPlayingPlayers;
            }
        }
        else 
        {
            currentTurnIdx -= turnsToChange;
            if (currentTurnIdx < 0) 
            {
                currentTurnIdx += totalPlayingPlayers;
            }
        }
        
        currentTurn = (Turn)currentTurnIdx;
        Debug.Log($"Ahora es el turno de {currentTurnIdx}");

        StartCoroutine(CheckIfPlus2OrPlus4WasPlayed());
    }

    private IEnumerator CheckIfPlus2OrPlus4WasPlayed()
    {
        if (totalCardsToDraw > 0)
        {
            Player currentPlayer = players[(int)currentTurn];
            Debug.Log($"currentPlayer: {currentPlayer}");
            
            bool hasToDraw = !currentPlayer.CanPlayAnyCard();
            Debug.Log($"Tengo que robar? {hasToDraw}");

            if (hasToDraw)
            {
                // El nuevo jugador tiene que robar cartas
                for (int i = 0; i < totalCardsToDraw; i++)
                {
                    currentPlayer.DrawCardToPlayerHand();
                }

                totalCardsToDraw = 0;
            }
            else
            {
                if (currentPlayer.GetIsMainPlayer()) yield break;
                
                yield return new WaitForSeconds(1);
                // Tengo que jugar el / los PLUS2 (del mismo color) 
                // TODO: Qué pasa si cardPlus2ToPlay es null?
                Card cardPlus2ToPlay = currentPlayer.FindCardInHand(CardType.Plus2);
                currentPlayer.PlayCard(cardPlus2ToPlay);
                
            }
            
            ChangeTurn();
        }
    }

    public void ChangeTurnOrder()
    {
        turnOrderClockwise = !turnOrderClockwise;
        Debug.Log($"Ahora el sentido es en sentido horario: {turnOrderClockwise}");
    }

    public void UpdateTotalCardsToDraw(int cardsToDraw)
    {
        totalCardsToDraw += cardsToDraw;
    }

    public int GetTotalCardsToDraw()
    {
        return totalCardsToDraw;
    }
}
