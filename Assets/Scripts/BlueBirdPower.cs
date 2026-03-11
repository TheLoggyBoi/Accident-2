using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBirdPower : MonoBehaviour
{
    // Reference to the Prefab you want to instantiate
    public GameObject objectToDuplicate;

    void Update()
    {
        // Check if the 'Space' key is pressed down once
        if (Input.GetKeyDown(KeyCode.D))
        {
            // Instantiate a clone of the prefab at the spawner's position and rotation
            Instantiate(objectToDuplicate, transform.position, transform.rotation);
        }
    }
}
