using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class birchose : MonoBehaviour, IPointerDownHandler, ISubmitHandler
{
    [Header("References")]
    public Dropdown dropdown;                  // Assign in Inspector (or put this script on the Dropdown)
    public Transform objectToMove;             // The GameObject you want to move
    [Tooltip("Targets must be in the same order as the dropdown options")]
    public List<Transform> locations = new();  // Create empty Transforms in scene as markers

    [Header("Behavior")]
    [SerializeField] private bool applyOnStart = false; // if true, move once on start to current value

    private bool armed = false;     // becomes true only after user interaction
    private bool subscribed = false;

    private void Awake()
    {
        if (dropdown == null)
            dropdown = GetComponent<Dropdown>();
    }

    private void OnEnable()
    {
        // Defer subscribing one frame so init-time value changes don't trigger us.
        StartCoroutine(DeferredSubscribe());
    }

    private IEnumerator DeferredSubscribe()
    {
        yield return null; // wait 1 frame
        if (dropdown != null && !subscribed)
        {
            dropdown.onValueChanged.AddListener(OnChanged);
            subscribed = true;
        }

        if (applyOnStart && dropdown != null)
        {
            Teleport(dropdown.value);
        }
    }

    private void OnDisable()
    {
        if (dropdown != null && subscribed)
        {
            dropdown.onValueChanged.RemoveListener(OnChanged);
            subscribed = false;
        }
        armed = false;
    }

    // Mouse/touch opens the dropdown -> arm handling
    public void OnPointerDown(PointerEventData eventData) => armed = true;

    // Keyboard/gamepad submit -> arm handling
    public void OnSubmit(BaseEventData eventData) => armed = true;

    private void OnChanged(int index)
    {
        // Ignore any value changes until the user has interacted with the dropdown
        if (!armed) return;
        Teleport(index);
    }

    private void Teleport(int index)
    {
        if (objectToMove == null || index < 0 || index >= locations.Count) return;
        objectToMove.position = locations[index].position;
        // Optional: also match rotation
        // objectToMove.rotation = locations[index].rotation;
    }
}