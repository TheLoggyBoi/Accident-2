using UnityEngine;

public class resetteleporter : MonoBehaviour
{
    
    public Transform resetpoint;

    public void TeleportObject()
    {
        transform.position = resetpoint.position;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}

