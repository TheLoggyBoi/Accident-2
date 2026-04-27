using UnityEngine;
using System.Collections;

public class bluebirdpower : MonoBehaviour
{
    [Header("Split Settings")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float spreadAngle = 20f;
    [SerializeField] private KeyCode abilityKey = KeyCode.Space;
    [SerializeField] private float cloneLifetime = 5f; // auto-destroy after this many seconds

    private Rigidbody rb;
    private bool hasActivated = false;
    private SlingShotController sc;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sc = GetComponent<SlingShotController>();
    }

    void Update()
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

        if (!Input.GetKeyDown(abilityKey)) return;

        Debug.Log($"Blue bird ability activated! Player: {(sc != null ? sc.GetPlayerNumber() : 0)}");
        Activate();
    }

    void Activate()
    {
        hasActivated = true;

        Vector3 velocity = rb.linearVelocity;
        if (velocity.sqrMagnitude < 0.01f)
        {
            Debug.LogWarning("Blue bird velocity too low to activate ability");
            return;
        }

        Debug.Log($"Blue bird ability activating with velocity: {velocity}");

        Vector3 leftVelocity = Quaternion.AngleAxis(-spreadAngle, Vector3.up) * velocity;
        Vector3 rightVelocity = Quaternion.AngleAxis(spreadAngle, Vector3.up) * velocity;

        StartCoroutine(SpawnClone(leftVelocity));
        StartCoroutine(SpawnClone(rightVelocity));
    }

    IEnumerator SpawnClone(Vector3 targetVelocity)
    {
        if (clonePrefab == null)
        {
            Debug.LogWarning("blueBirdPower: clonePrefab is not assigned!");
            yield break;
        }

        GameObject clone = Instantiate(clonePrefab, transform.position, transform.rotation);

        // Strip slingshot/network components so nothing resets isKinematic
        foreach (var s in clone.GetComponents<SlingShotController>()) Destroy(s);
        foreach (var nb in clone.GetComponents<Unity.Netcode.NetworkBehaviour>()) Destroy(nb);
        foreach (var no in clone.GetComponents<Unity.Netcode.NetworkObject>()) Destroy(no);
        foreach (var bp in clone.GetComponents<bluebirdpower>()) Destroy(bp);

        clone.tag = "Bird";

        if (clone.GetComponent<Collider>() == null)
        {
            SphereCollider col = clone.AddComponent<SphereCollider>();
            col.radius = 0.5f;
        }

        Rigidbody cloneRb = clone.GetComponent<Rigidbody>();
        if (cloneRb == null)
            cloneRb = clone.AddComponent<Rigidbody>();

        cloneRb.isKinematic = false;
        cloneRb.useGravity = true;

        yield return new WaitForFixedUpdate();

        cloneRb.isKinematic = false;
        cloneRb.linearVelocity = targetVelocity;

        Bluebirdclone cloneScript = clone.GetComponent<Bluebirdclone>();
        if (cloneScript == null)
            cloneScript = clone.AddComponent<Bluebirdclone>();

        cloneScript.playerNumber = sc != null ? sc.GetPlayerNumber() : 1;

        // Auto-destroy so the clone doesn't linger on screen forever
        Destroy(clone, cloneLifetime);

        Debug.Log($"Blue bird clone launched with velocity {targetVelocity}");
    }
}
