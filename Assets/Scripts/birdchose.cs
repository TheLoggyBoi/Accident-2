using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Birdchose : MonoBehaviour
{
    [Header("References")]
    public Dropdown dropdown;
    public Transform objectToMove;
    [Tooltip("Targets must be in the same order as the dropdown options")]
    public List<Transform> locations = new();

    private int lastIndex = -1; // track last applied index
    private bool initialized = false;

    private void Awake()
    {
        if (dropdown == null)
            dropdown = GetComponent<Dropdown>();
    }

    private void Start()
    {
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        // Wait 2 frames to let Unity finish all initialization
        yield return null;
        yield return null;

        if (dropdown != null)
        {
            // Store initial value but DON'T teleport
            lastIndex = dropdown.value;
            dropdown.onValueChanged.AddListener(OnChanged);
        }

        initialized = true;
    }

    private void OnDisable()
    {
        if (dropdown != null)
            dropdown.onValueChanged.RemoveListener(OnChanged);
    }

    private void OnChanged(int index)
    {
        // Only teleport if:
        // 1. We're fully initialized
        // 2. The index actually changed
        if (!initialized) return;
        if (index == lastIndex) return;

        Teleport(index);
        lastIndex = index;
    }

    private void Teleport(int index)
    {
        if (objectToMove == null || index < 0 || index >= locations.Count) return;
        objectToMove.position = locations[index].position;
        // objectToMove.rotation = locations[index].rotation;
    }

}