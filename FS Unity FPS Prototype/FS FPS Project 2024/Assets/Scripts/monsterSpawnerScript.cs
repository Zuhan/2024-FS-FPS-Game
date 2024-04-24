using System.Collections;
using UnityEngine;
public class monsterSpawnerScript : MonoBehaviour
{
    public GameObject creatureSpawn;
    public float baseSpawnRate = 2f;
    public float maxSpawnRate = 0.5f;
    public float minScale = 0.5f;
    public float maxScale = 1.0f;
    public float scaleRestoreRate = 0.1f;
    public float timer = 0f;

    // Start is called before the first frame update
    private Coroutine spawnCoroutine;

    void Start()
    {
        // Start the coroutine to manage spawn timing
        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float currentScale = transform.localScale.x;
            float scaleRatio = (currentScale - minScale) / (maxScale - minScale);
            float currentSpawnRate = Mathf.Lerp(maxSpawnRate, baseSpawnRate, scaleRatio);

            yield return new WaitForSeconds(currentSpawnRate); // Wait before spawning

            SpawnCreature(); // Spawn the creature

            // Gradually restore scale to the maximum
            if (transform.localScale.x < maxScale)
            {
                transform.localScale += Vector3.one * scaleRestoreRate * currentSpawnRate; // Restore over time
            }
        }
    }

    void SpawnCreature()
    {
        Instantiate(creatureSpawn, transform.position, transform.rotation);
    }

    public void ScaleDown()
    {
        if (transform.localScale.x > minScale) // Ensure it doesn't go below minScale
        {
            transform.localScale -= Vector3.one * 0.1f; // Scale down a bit
        }
    }
}
