using UnityEngine;

public class blueBirdPower : MonoBehaviour
{
    // Public variable to hold the Prefab reference.
    // The [SerializeField] attribute makes a private variable visible in the Inspector.
    [SerializeField] private GameObject prefabToSpawn;

    // Public variable to define the spawn point. Assign this in the Inspector.
    [SerializeField] private Transform spawnPoint;

    void Update()
    {
        // Check if the specified key (e.g., Spacebar) is pressed down.
        if (Input.GetKeyDown(KeyCode.D))
        {
            // Call the function to spawn the object.
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        // Instantiate the prefab at the spawner's position and rotation.
        // Quaternion.identity means no rotation (original rotation of the prefab asset).
        Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);
    }
}
