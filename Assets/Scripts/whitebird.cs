using UnityEngine;

public class whitebird : MonoBehaviour
{
    [Header("Settings")]
    public Rigidbody rb;
    public float downwardPushforce = 10f;
    
    [Header("Input Options")]
    [Tooltip("Primary key to activate ability")]
    public KeyCode abilityKey = KeyCode.Space;
    [Tooltip("Allow mouse click to activate ability")]
    public bool allowMouseClick = true;
    
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

        // Check for keyboard input
        bool keyPressed = Input.GetKeyDown(abilityKey);
        
        // Check for mouse click input (if enabled)
        bool mouseClicked = allowMouseClick && Input.GetMouseButtonDown(0);

        if (keyPressed || mouseClicked)
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
    
    // Public method that can be called from UI buttons
    public void ActivateAbility()
    {
        if (!hasActivated && rb != null && !rb.isKinematic)
        {
            Debug.Log($"White bird ability activated via button! Player: {(sc != null ? sc.GetPlayerNumber() : 0)}");
            applydownwardPush();
        }
    }
}

