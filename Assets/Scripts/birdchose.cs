using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class birchose : MonoBehaviour
{
    [Header("References")]
    public Dropdown dropdown;            // Assign in Inspector (or put this script on the Dropdown)
    public Transform objectToMove;       // The GameObject you want to move

    [Tooltip("Targets must be in the same order as the dropdown options")]
    public List<Transform> locations = new();  // Create empty Transforms in scene as markers

    private void Awake()
    {
        if (dropdown == null)
            dropdown = GetComponent<Dropdown>();
    }

    private void OnEnable()
    {
        if (dropdown != null)
            dropdown.onValueChanged.AddListener(OnChanged);
    }

    private void OnDisable()
    {
        if (dropdown != null)
            dropdown.onValueChanged.RemoveListener(OnChanged);
    }

    private void Start()
    {
        // Apply current selection on start
        if (dropdown != null)
            OnChanged(dropdown.value);
    }

    private void OnChanged(int index)
    {
        if (objectToMove == null || index < 0 || index >= locations.Count) return;

        // Teleport
        objectToMove.position = locations[index].position;
        // Optional: also match rotation
        // objectToMove.rotation = locations[index].rotation;
    }
}