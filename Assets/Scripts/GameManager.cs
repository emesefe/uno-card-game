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
    private Player? whoHasPressedUnoButton;

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
        StartNewGame();
    }

    private void StartNewGame()
    {
        CardsManager.Instance.CreateDrawDeck();
        CardsManager.Instance.ShuffleDrawDeck();
        
        currentTurn = Turn.Player01; // TODO: Make this random
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

    #region TURN
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
        
        OnTurnChanged?.Invoke(currentTurn);
        
        if (changingTurns) return;
        
        StartNextTurn();
    }

    private void StartNextTurn()
    {
        Player currentPlayer = players[(int)currentTurn];

        StartCoroutine(totalCardsToDraw > 0
            ? CheckIfPlus2OrPlus4WasPlayed(currentPlayer)
            : CheckIfCanPlayCard(currentPlayer));
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
        bool hasToDraw = !currentPlayer.CanPlayAnyCard();

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

    public void UpdateTotalCardsToDraw(int cardsToDraw)
    {
        totalCardsToDraw += cardsToDraw;
        UIManager.Instance.UpdateTotalCardsToDraw(totalCardsToDraw);
    }
    
    public void ChangeTurnOrder()
    {
        turnOrderClockwise = !turnOrderClockwise;
        Debug.Log($"Ahora el sentido es en sentido horario: {turnOrderClockwise}");
    }
    
    public void SetChangingTurns(bool changing)
    {
        changingTurns = changing;
    }
    #endregion

    #region COLOR
    public void ChangeCurrentColor(CardColor color)
    {
        currentColor = color;
        
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
    #endregion

    #region UNO
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
        float secondsToClickUNOButton = Random.Range(1.5f, 3f);
        yield return new WaitForSeconds(secondsToClickUNOButton);
        
        int randomPlayerIdx = Random.Range(1, totalPlayingPlayers);
        SetUNOButtonHasBeenPressed(true, randomPlayerIdx);
    }
    #endregion
}
