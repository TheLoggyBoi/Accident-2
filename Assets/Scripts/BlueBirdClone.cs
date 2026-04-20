using UnityEngine;

// Automatically added to blue bird clones.
// Handles tic-tac-toe square collision so clones can claim squares.
public class BlueBirdClone : MonoBehaviour
{
    public int playerNumber = 1;
    private bool hasHit = false;

    void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;
        TryClaimSquare(collision.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;
        TryClaimSquare(other.gameObject);
    }

    void TryClaimSquare(GameObject target)
    {
        // Check the hit object or its parent for a TicTacToeSquare
        TicTacToeSquare square = target.GetComponent<TicTacToeSquare>()
            ?? target.GetComponentInParent<TicTacToeSquare>();

        if (square != null)
        {
            hasHit = true;
            square.OnSquareHit(playerNumber);
            Debug.Log($"Blue bird clone claimed square for player {playerNumber}");
        }
    }
}
