using UnityEngine;
using UnityEngine.UI;

public class BirdChoice : MonoBehaviour
{
    [Header("Teleport Settings")]
    public GameObject objectToTeleport;
    public Transform targetLocation;

    // Alternative: use specific coordinates instead
    public bool useCoordinates = false;
    public Vector3 targetPosition;

    [Header("UI Button (Optional)")]
    public Button teleportButton;

    void Start()
    {
        // If you assigned a button, hook it up
        if (teleportButton != null)
        {
            teleportButton.onClick.AddListener(TeleportObject);
        }
    }

    // Call this method from a UI button or other trigger
    public void TeleportObject()
    {
        if (objectToTeleport == null)
        {
            Debug.LogWarning("No object assigned to teleport!");
            return;
        }

        if (useCoordinates)
        {
            objectToTeleport.transform.position = targetPosition;
        }
        else if (targetLocation != null)
        {
            objectToTeleport.transform.position = targetLocation.position;
        }
        else
        {
            Debug.LogWarning("No target location or coordinates set!");
        }

        Debug.Log($"Teleported {objectToTeleport.name} to {objectToTeleport.transform.position}");
    }
}