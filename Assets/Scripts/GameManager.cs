#nullable enable
using UnityEngine;
using System;
using System.Collections;

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
        CardsManager.Instance.ShuffleDeck();
        
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

    private IEnumerator CheckIfCanPlayCard(Player currentPlayer)
    {
        if (IsCurrentPlayerMainPlayer()) yield break;
        
        yield return new WaitForSeconds(3);
            
        Card? cardToPlay = currentPlayer.GetFirstCardThatCanBePlayed();
        while (cardToPlay == null)
        {
            currentPlayer.DrawCardToPlayerHand();
            cardToPlay = currentPlayer.GetFirstCardThatCanBePlayed();
        }
        
        currentPlayer.PlayCard(cardToPlay);
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
            
            yield return new WaitForSeconds(3);
            
            CardType? typeToPlay = currentPlayer.GetFirstCardTypeThatCanBePlayed();
            if (typeToPlay != null)
            {
                Card cardToPlay = currentPlayer.FindCardInHand(typeToPlay.Value);
                currentPlayer.PlayCard(cardToPlay);
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
}
