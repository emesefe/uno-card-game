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

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one instance of UIManager");
        }

        Instance = this;
        
        InitializeConfirmSelectionButton();
    }

    private void InitializeConfirmSelectionButton()
    {
        confirmSelectionButton.onClick.AddListener(player.PlaySelectedCards);
        EnableConfirmSelectionButton(false);
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
            Debug.Log("Shaking time: " + shakeTime);
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
}
