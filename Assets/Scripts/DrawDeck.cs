using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DrawDeck : MonoBehaviour
{
    [SerializeField] Player player;

    private void OnMouseDown()
    {
        Debug.Log("Quiero robar");

        if (!player.CanPlayAnyCard()) player.DrawCardToPlayerHand();
    }

}
