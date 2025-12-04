using UnityEngine;

public class reset : MonoBehaviour
{
    public Transform bird;
    public Vector3 resetpoint;
    public void TeleportObject()
    {
        if (bird != null)
        {
            bird.position = resetpoint;
        }
        else
        {
            Debug.LogWarning("Object to Teleport is not assigned in the Teleporter script.");
        }
    }
}
