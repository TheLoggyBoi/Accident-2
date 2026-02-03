using UnityEngine;

public class teleporter : MonoBehaviour
{
    public Transform destination;
    
    public void teleportobject()
    {
        transform.position = destination.position;

        Rigidbody rb = transform.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
