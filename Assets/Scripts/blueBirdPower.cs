using UnityEngine;

public class blueBirdPower : MonoBehaviour
{
    // Reference to the prefab you want to spawn.
    // Make sure to assign this in the Unity Inspector!
    public GameObject objectToSpawnPrefab;

    // Reference to a transform component, which dictates where the object spawns.
    // Assign an empty GameObject in the scene to this in the Inspector.
    public Transform spawnPoint;

    // This public function will be called by the UI button.
    public void SpawnObject()
    {
        // Instantiate a clone of the prefab at the spawner's position and rotation.
        Instantiate(objectToSpawnPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
