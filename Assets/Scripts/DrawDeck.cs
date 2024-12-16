using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DrawDeck : MonoBehaviour
{
    [SerializeField] Player mainPlayer;

    private void OnMouseDown()
    {
        if (!mainPlayer.CanPlayAnyCard()) mainPlayer.DrawCardToPlayerHand();
    }

}
