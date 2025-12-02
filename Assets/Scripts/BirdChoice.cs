using UnityEngine;
using UnityEngine.UI;

public class BirdChoice : MonoBehaviour
{
    public Transform bird;
    public Vector3 singshot;

    public void TeleportObject()
    {
        if (bird != null)
        {
            bird.position = singshot;
        }
        else
        {
            Debug.LogWarning("Object to Teleport is not assigned in the Teleporter script.");
        }
    }
}