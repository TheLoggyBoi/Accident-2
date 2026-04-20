using UnityEngine;

// Attach this to the blue bird (SlingShotController GameObject).
// When the bird is in flight, press the ability key to split into clones
// that fan out and follow the same trajectory as the original.
public class blueBirdPower : MonoBehaviour
{
    [Header("Split Settings")]
    [SerializeField] private GameObject clonePrefab;   // Assign the blue bird prefab
    [SerializeField] private int cloneCount = 2;        // How many extra birds to spawn
    [SerializeField] private float spreadAngle = 15f;   // Degrees between each clone
    [SerializeField] private KeyCode abilityKey = KeyCode.Space;

    private Rigidbody rb;
    private bool hasActivated = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Only activate once, while the bird is actually flying (not kinematic)
        if (hasActivated) return;
        if (rb == null || rb.isKinematic) return;
        if (!Input.GetKeyDown(abilityKey)) return;

        Activate();
    }

    void Activate()
    {
        hasActivated = true;

        Vector3 velocity = rb.linearVelocity;
        if (velocity == Vector3.zero) return;

        // Spread clones evenly around the original direction
        // e.g. 2 clones at -15 and +15 degrees
        float startAngle = -(cloneCount - 1) * spreadAngle / 2f;

        for (int i = 0; i < cloneCount; i++)
        {
            float angle = startAngle + i * spreadAngle;
            SpawnClone(velocity, angle);
        }
    }

    void SpawnClone(Vector3 originalVelocity, float angleOffset)
    {
        if (clonePrefab == null)
        {
            Debug.LogWarning("blueBirdPower: clonePrefab is not assigned!");
            return;
        }

        // Rotate the velocity around the Z axis (up/down spread) or Y axis (left/right)
        // Using Z gives a vertical fan; swap to Y for a horizontal fan
        Quaternion rotation = Quaternion.AngleAxis(angleOffset, Vector3.forward);
        Vector3 cloneVelocity = rotation * originalVelocity;

        GameObject clone = Instantiate(clonePrefab, transform.position, transform.rotation);

        // Give the clone the same speed in its new direction
        Rigidbody cloneRb = clone.GetComponent<Rigidbody>();
        if (cloneRb == null)
            cloneRb = clone.AddComponent<Rigidbody>();

        cloneRb.isKinematic = false;
        cloneRb.linearVelocity = cloneVelocity;

        // Make sure the clone has a collider
        if (clone.GetComponent<Collider>() == null)
        {
            SphereCollider col = clone.AddComponent<SphereCollider>();
            col.radius = 0.5f;
        }

        // Tag it as a bird so tic-tac-toe squares can detect it
        clone.tag = "Bird";

        // Attach the clone follower so it can claim squares on hit
        BlueBirdClone cloneScript = clone.GetComponent<BlueBirdClone>();
        if (cloneScript == null)
            cloneScript = clone.AddComponent<BlueBirdClone>();

        cloneScript.playerNumber = GetComponent<SlingShotController>() != null
            ? GetComponent<SlingShotController>().GetPlayerNumber()
            : 1;

        Debug.Log($"Blue bird spawned clone with velocity {cloneVelocity}");
    }
}
