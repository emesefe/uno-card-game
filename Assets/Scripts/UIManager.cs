using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button confirmSelectionButton;

    [SerializeField] private Player player;

    private void Awake()
    {
        confirmSelectionButton.onClick.AddListener(player.PlaySelectedCards);
    }
}
