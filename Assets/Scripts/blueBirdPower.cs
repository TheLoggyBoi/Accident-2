using UnityEngine;
using System.Collections;

// Attach this to the blue bird (SlingShotController GameObject).
// Press the ability key while in flight to split into a left and right clone.
public class blueBirdPower : MonoBehaviour
{
    [Header("Split Settings")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float spreadAngle = 20f;   // Degrees left/right from travel direction
    [SerializeField] private KeyCode abilityKey = KeyCode.Space;

    private Rigidbody rb;
    private bool hasActivated = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (hasActivated) return;
        if (rb == null || rb.isKinematic) return;
        if (!Input.GetKeyDown(abilityKey)) return;

        Activate();
    }

    void Activate()
    {
        hasActivated = true;

        Vector3 velocity = rb.linearVelocity;
        if (velocity.sqrMagnitude < 0.01f) return;

        // Rotate left and right around the Y axis (horizontal spread)
        // so one clone goes left and one goes right relative to travel direction
        Vector3 leftVelocity  = Quaternion.AngleAxis(-spreadAngle, Vector3.up) * velocity;
        Vector3 rightVelocity = Quaternion.AngleAxis( spreadAngle, Vector3.up) * velocity;

        StartCoroutine(SpawnClone(leftVelocity));
        StartCoroutine(SpawnClone(rightVelocity));
    }

    // Coroutine so we can set velocity on the next fixed update,
    // which is required for a freshly instantiated Rigidbody to actually move.
    IEnumerator SpawnClone(Vector3 targetVelocity)
    {
        if (clonePrefab == null)
        {
            Debug.LogWarning("blueBirdPower: clonePrefab is not assigned!");
            yield break;
        }

        GameObject clone = Instantiate(clonePrefab, transform.position, transform.rotation);

        // Ensure tag is set for board collision detection
        clone.tag = "Bird";

        // Ensure it has a collider
        if (clone.GetComponent<Collider>() == null)
        {
            SphereCollider col = clone.AddComponent<SphereCollider>();
            col.radius = 0.5f;
        }

        // Get or add rigidbody
        Rigidbody cloneRb = clone.GetComponent<Rigidbody>();
        if (cloneRb == null)
            cloneRb = clone.AddComponent<Rigidbody>();

        // Must be non-kinematic before setting velocity
        cloneRb.isKinematic = false;

        // Wait one fixed frame so Unity registers the rigidbody properly
        yield return new WaitForFixedUpdate();

        if (cloneRb != null)
            cloneRb.linearVelocity = targetVelocity;

        // Add the clone collision handler
        BlueBirdClone cloneScript = clone.GetComponent<BlueBirdClone>();
        if (cloneScript == null)
            cloneScript = clone.AddComponent<BlueBirdClone>();

        SlingShotController sc = GetComponent<SlingShotController>();
        cloneScript.playerNumber = sc != null ? sc.GetPlayerNumber() : 1;

        Debug.Log($"Blue bird clone launched with velocity {targetVelocity}");
    }
}
