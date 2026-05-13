using UnityEngine;

public class whitebird : MonoBehaviour
{
    public Rigidbody rb;
    public float downwardPushforce = 10f;
    
    private bool hasActivated = false;
    private SlingShotController sc;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sc = GetComponent<SlingShotController>();
    }

    public void Update()
    {
        if (hasActivated) return;
        if (rb == null || rb.isKinematic) return;

        // Check if it's this player's turn via TurnManager
        if (sc != null)
        {
            // Only allow ability if it's the player's turn
            if (TurnManager.Instance != null)
            {
                int currentPlayer = TurnManager.Instance.GetCurrentPlayer();
                int myPlayerNumber = TurnManager.Instance.GetMyPlayerNumber();
                int birdPlayerNumber = sc.GetPlayerNumber();
                
                // Only activate if it's my turn AND this is my bird
                if (currentPlayer != myPlayerNumber || birdPlayerNumber != myPlayerNumber)
                {
                    return;
                }
            }
            
            // Don't activate if the bird is being dragged
            if (sc.IsDragging()) return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"White bird ability activated! Player: {(sc != null ? sc.GetPlayerNumber() : 0)}");
            applydownwardPush();
        }
    }

    public void applydownwardPush()
    {
        if (rb != null && !hasActivated)
        {
            hasActivated = true;
            
            // Apply downward force without zeroing out existing velocity
            // This keeps the bird's momentum while adding downward push
            rb.AddForce(Vector3.down * downwardPushforce, ForceMode.Impulse);
            
            Debug.Log($"White bird pushed down with force: {downwardPushforce}");
        }
    }
}
