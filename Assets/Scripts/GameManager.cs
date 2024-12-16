#nullable enable
using UnityEngine;
using System;
using System.Collections;
using Random = UnityEngine.Random;

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

public class GameManager : MonoBehaviour
{
    [SerializeField] private SOCard soCard;
    
    public static GameManager Instance;
    
    public static event Action<Turn> OnTurnChanged;

    [SerializeField] private Player[] players;
    private int totalPlayingPlayers = 2;

    private Turn currentTurn;
    private bool turnOrderClockwise;
    private bool changingTurns;
    
    private int totalCardsToDraw;

    private CardColor currentColor;
    private bool colorHasBeenChanged;

    private bool unoButtonHasBeenPressed;
    private Player whoHasPressedUnoButton;

    private float timeToWaitForPlayerToPlay = 3f;

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
        
        // Inicializar partida
        CardsManager.Instance.CreateDrawDeck();
        CardsManager.Instance.ShuffleDrawDeck();
        
        turnOrderClockwise = true;
        totalCardsToDraw = 0;

        foreach (Player player in players)
        {
            player.InitializePlayerHand();
        }

        Card firstPlayedCard = CardsManager.Instance.DrawCardFromDrawDeck();
        CardsManager.Instance.AddCardToDiscardDeck(firstPlayedCard);

        if (!firstPlayedCard.IsPlus4OrChangeColor())
        {
            UIManager.Instance.HideCurrentColorText();
            currentColor = firstPlayedCard.GetColor();
        }
        
        else {
            CardColor randomCardColor = CardColors.ChooseRandomColor();
            UIManager.Instance.ShowCurrentColorText(randomCardColor);
            ChangeCurrentColor(randomCardColor);
        }
        
        Debug.Log($"Empezamos con el color:{currentColor}");
    }

    public void ChangeTurn(int turnsToChange = 1)
    {
        int currentTurnIdx = (int)currentTurn; 
        
        bool gameOver = CheckIfCurrentPlayerHasWon(currentTurnIdx);
        if (gameOver)
        {
            UIManager.Instance.ShowWinPanel();
            return;
        }
        
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
        
        OnTurnChanged?.Invoke(currentTurn);
        
        if (changingTurns) return;
        
        // He empezado el siguiente turno
        StartNextTurn();
    }

    private void StartNextTurn()
    {
        Player currentPlayer = players[(int)currentTurn];
        
        if (totalCardsToDraw > 0)
        {
            StartCoroutine(CheckIfPlus2OrPlus4WasPlayed(currentPlayer));
        }
        else
        {
            StartCoroutine(CheckIfCanPlayCard(currentPlayer));
        }
    }

    private bool CheckIfCurrentPlayerHasWon(int currentTurnIdx)
    {
        Player currentPlayer = players[currentTurnIdx];
        return currentPlayer.CheckIfHasWon();
    }
    
    private bool CheckIfCurrentPlayerHasUNO(int currentTurnIdx)
    {
        Player currentPlayer = players[currentTurnIdx];
        return currentPlayer.CheckUNO();
    }

    private IEnumerator CheckIfCanPlayCard(Player currentPlayer)
    {
        if (IsCurrentPlayerMainPlayer()) yield break;
        
        yield return new WaitForSeconds(timeToWaitForPlayerToPlay);
            
        Card? cardToPlay = currentPlayer.GetFirstCardThatCanBePlayed();
        while (cardToPlay == null)
        {
            currentPlayer.DrawCardToPlayerHand();
            cardToPlay = currentPlayer.GetFirstCardThatCanBePlayed();
        }
        
        StartCoroutine(currentPlayer.PlayCard(cardToPlay));
        
        
        SetChangingTurns(false);
        ChangeTurn();
    }

    private IEnumerator CheckIfPlus2OrPlus4WasPlayed(Player currentPlayer)
    {
        Debug.Log($"currentPlayer: {currentPlayer}");
        
        bool hasToDraw = !currentPlayer.CanPlayAnyCard();
        Debug.Log($"Tengo que robar? {hasToDraw}");

        if (hasToDraw)
        {
            for (int i = 0; i < totalCardsToDraw; i++)
            {
                currentPlayer.DrawCardToPlayerHand();
            }

            totalCardsToDraw = 0;
            UIManager.Instance.HideTotalCardsToDraw(2f, 2f);
        }
        else
        {
            if (IsCurrentPlayerMainPlayer()) yield break;
            
            yield return new WaitForSeconds(timeToWaitForPlayerToPlay);
            
            CardType? typeToPlay = currentPlayer.GetFirstCardTypeThatCanBePlayed();
            if (typeToPlay != null)
            {
                Card cardToPlay = currentPlayer.FindCardInHand(typeToPlay.Value);
                StartCoroutine(currentPlayer.PlayCard(cardToPlay));
            }
            
            
        }
        
        ChangeTurn();
        
    }

    public void ChangeTurnOrder()
    {
        turnOrderClockwise = !turnOrderClockwise;
        Debug.Log($"Ahora el sentido es en sentido horario: {turnOrderClockwise}");
    }

    public void UpdateTotalCardsToDraw(int cardsToDraw)
    {
        totalCardsToDraw += cardsToDraw;
        UIManager.Instance.UpdateTotalCardsToDraw(totalCardsToDraw);
        Debug.Log($"Se tienen que robar: {totalCardsToDraw} cartas");
    }

    public int GetTotalCardsToDraw()
    {
        return totalCardsToDraw;
    }

    public Player GetCurrentPlayer()
    {
        return players[(int)currentTurn];
    }

    public bool IsCurrentPlayerMainPlayer()
    {
        return GetCurrentPlayer().IsMainPlayer();
    }

    public void ChangeCurrentColor(CardColor color)
    {
        currentColor = color;
        Debug.Log($"Cambio al color {color}");
        
        colorHasBeenChanged = true;
        
        UIManager.Instance.HideCurrentColorText(3f);
    }

    public CardColor GetCurrentColor()
    {
        return currentColor;
    }

    public bool GetColorHasBeenChanged()
    {
        return colorHasBeenChanged;
    }

    public void SetColorHasBeenChanged(bool hasBeenChanged)
    {
        colorHasBeenChanged = hasBeenChanged;
    }
    
    public void SetChangingTurns(bool changing)
    {
        changingTurns = changing;
    }

    public bool GetChangingTurns()
    {
        return changingTurns;
    }
    
    public bool GetUNOButtonHasBeenPressed()
    {
        return unoButtonHasBeenPressed;
    }

    public Player GetWhoHasPressedUnoButton()
    {
        return whoHasPressedUnoButton;
    }

    public void SetUNOButtonHasBeenPressed(bool hasBeenPressed, int whoHasPressedIdx)
    {
        unoButtonHasBeenPressed = hasBeenPressed;
        whoHasPressedUnoButton = whoHasPressedIdx < 0 ? null : players[whoHasPressedIdx];
    }

    public IEnumerator UNOTimer()
    {
        // TODO: Make this timer random
        yield return new WaitForSeconds(3f);
        
        int randomPlayerIdx = Random.Range(1, totalPlayingPlayers);
        SetUNOButtonHasBeenPressed(true, randomPlayerIdx);
    }
}
