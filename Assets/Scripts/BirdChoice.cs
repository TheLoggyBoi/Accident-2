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

        Vector3 newPosition;

        if (useCoordinates)
        {
            newPosition = targetPosition;
        }
        else if (targetLocation != null)
        {
            newPosition = targetLocation.position;
        }
        else
        {
            Debug.LogWarning("No target location or coordinates set!");
            return;
        }

        // Teleport the object
        objectToTeleport.transform.position = newPosition;

        // Update any stored original positions in other scripts
        SlingShotController slingshot = objectToTeleport.GetComponent<SlingShotController>();
        if (slingshot != null)
        {
            // Reset the bird to update its internal start position
            slingshot.ResetBird();
        }

        Debug.Log($"Teleported {objectToTeleport.name} to {newPosition}");
    }
}