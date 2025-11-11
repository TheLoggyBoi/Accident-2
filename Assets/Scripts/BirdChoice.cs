using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdChoice : MonoBehaviour
{
    [Header("References")]
    public Dropdown dropdown;
    public Transform objectToMove;       // The specific object THIS dropdown controls

    [Tooltip("Targets must be in the same order as the dropdown options")]
    public List<Transform> locations = new();

    private void Awake()
    {
        if (dropdown == null)
            dropdown = GetComponent<Dropdown>();
    }

    private void Start()
    {
        if (dropdown != null)
            dropdown.onValueChanged.AddListener(OnChanged);
    }

    private void OnDestroy()
    {
        if (dropdown != null)
            dropdown.onValueChanged.RemoveListener(OnChanged);
    }

    private void OnChanged(int index)
    {
        if (objectToMove == null || index < 0 || index >= locations.Count)
        {
            Debug.LogWarning($"[BirdChoice] Cannot teleport - invalid index {index} or missing references", this);
            return;
        }

        Debug.Log($"[BirdChoice] Teleporting {objectToMove.name} to {locations[index].name} (index {index})", this);
        objectToMove.position = locations[index].position;
        // objectToMove.rotation = locations[index].rotation;
    }
}