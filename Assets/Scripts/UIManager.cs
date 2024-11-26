using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    [SerializeField] private Button confirmSelectionButton;

    [SerializeField] private Player player;
    
    [SerializeField] private GameObject totalCardsToDraw;
    [SerializeField] private RectTransform totalCardsToDrawRectTransform;
    [SerializeField] private TextMeshProUGUI totalCardsToDrawText;
    [SerializeField] private CanvasGroup totalCardsCanvasGroup;

    [SerializeField] private GameObject changeColorPanel;
    [SerializeField] private Button[] changeColorButtons;
    [SerializeField] private TextMeshProUGUI currentColorText;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one instance of UIManager");
        }

        Instance = this;
        
        InitializeConfirmSelectionButton();
        
        HideChangeColorPanel();
        InitializeChangeColorButtons();
        
        HideCurrentColorText();
    }

    private void InitializeConfirmSelectionButton()
    {
        confirmSelectionButton.onClick.AddListener(() =>
            StartCoroutine(player.PlaySelectedCards()));
        EnableConfirmSelectionButton(false);
    }
    
    private void InitializeChangeColorButtons()
    {
        for (int i = 0; i < changeColorButtons.Length; i++)
        {
            CardColor cardColor = (CardColor)i;
            changeColorButtons[i].onClick.AddListener(() =>
            {
                GameManager.Instance.ChangeCurrentColor(cardColor);
                HideChangeColorPanel();
                ShowCurrentColorText(cardColor);
            });
        }
    }

    public void EnableConfirmSelectionButton(bool enable)
    {
        confirmSelectionButton.interactable = enable;
    }

    public void ShowTotalCardsToDraw(float fadeTime = 0)
    {
        totalCardsToDraw.SetActive(true);
        totalCardsCanvasGroup.DOFade(1, fadeTime);
    }

    private IEnumerator HideTotalCardsToDrawCoroutine(float fadeTime = 0, float shakeTime = 0)
    {
        if (shakeTime > 0)
        {
            totalCardsToDrawRectTransform.DOShakeAnchorPos(shakeTime, 20);
            yield return new WaitForSeconds(shakeTime);
        }
        
        totalCardsCanvasGroup.DOFade(0, fadeTime);
        yield return new WaitForSeconds(fadeTime);
        
        totalCardsToDraw.SetActive(false);
    }

    public void HideTotalCardsToDraw(float fadeTime = 0, float shakeTime = 0)
    {
        StartCoroutine(HideTotalCardsToDrawCoroutine(fadeTime, shakeTime));
    }

    public void UpdateTotalCardsToDraw(int cardsToDraw)
    {
        totalCardsToDrawText.text = cardsToDraw.ToString();
    }

    public void ShowChangeColorPanel()
    {
        changeColorPanel.SetActive(true);
    }

    private void HideChangeColorPanel()
    {
        changeColorPanel.SetActive(false);
    }

    public void ShowCurrentColorText(CardColor color, float fadeTime = 0)
    {
        currentColorText.gameObject.SetActive(true);
        currentColorText.color = Card.CardColors[color];
        currentColorText.text = color.ToString();
        
        currentColorText.DOFade(1, fadeTime);
    }
    
    private IEnumerator HideCurrentColorTextCoroutine(float fadeTime = 0)
    {
        currentColorText.DOFade(0, fadeTime);
        yield return new WaitForSeconds(fadeTime);
        
        currentColorText.gameObject.SetActive(false);
    }
    
    public void HideCurrentColorText(float fadeTime = 0)
    {
        StartCoroutine(HideCurrentColorTextCoroutine(fadeTime));
    }
}
