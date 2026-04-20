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

        // Strip any slingshot/network scripts immediately — before their Start() runs
        // so nothing can set isKinematic = true on us after we set it false.
        foreach (var sc in clone.GetComponents<SlingShotController>())
            Destroy(sc);
        foreach (var nb in clone.GetComponents<Unity.Netcode.NetworkBehaviour>())
            Destroy(nb);
        foreach (var no in clone.GetComponents<Unity.Netcode.NetworkObject>())
            Destroy(no);

        // Ensure tag is set for board collision detection
        clone.tag = "Bird";

        // Ensure it has a collider
        if (clone.GetComponent<Collider>() == null)
        {
            SphereCollider col = clone.AddComponent<SphereCollider>();
            col.radius = 0.5f;
        }

        // Get or add rigidbody and force non-kinematic immediately
        Rigidbody cloneRb = clone.GetComponent<Rigidbody>();
        if (cloneRb == null)
            cloneRb = clone.AddComponent<Rigidbody>();

        cloneRb.isKinematic = false;
        cloneRb.useGravity = true;

        // Wait one fixed frame then apply velocity
        yield return new WaitForFixedUpdate();

        // Re-assert in case anything snuck in during that frame
        cloneRb.isKinematic = false;
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
