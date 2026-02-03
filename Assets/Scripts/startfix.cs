using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Apple;

public class startfix : MonoBehaviour
{
    public Transform startpoint;
    void Start()
    {
     transform.position = startpoint.position;

     Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
