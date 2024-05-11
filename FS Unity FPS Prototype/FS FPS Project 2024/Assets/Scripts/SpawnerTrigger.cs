using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTrigger : MonoBehaviour
{
    [SerializeField] GameObject[] enemiesToSpawn;
    [SerializeField] int numToSpawn;
    [SerializeField] int spawnDelay;

    int spawnCount;
    bool isSpawning;
    bool startSpawning;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(startSpawning && !isSpawning && spawnCount < numToSpawn)
        {
            StartCoroutine(spawn());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startSpawning = true;
        }
    }
    IEnumerator spawn()
    {
        isSpawning = true;
        int spawnPos = Random.Range(0, transform.childCount);
        int enemy = Random.Range(0,enemiesToSpawn.Length);
        Instantiate(enemiesToSpawn[enemy], transform.GetChild(spawnPos).position, transform.GetChild(spawnPos).rotation);
        spawnCount++;
        yield return new WaitForSeconds(spawnDelay);
        isSpawning=false;
    }

    //algorithim to spawn enemies in a weighted way to make enemies less likely or more likely to spawn
    //not yet implemented fully
    private int enemyAlgorithim()
    {
        int enemy = Random.Range(0, 100);
        int num = 0;
        if(enemy <= 19)
        {
            num = 0;
            return num;
        }
        else if(enemy <= 69)
        {
            num = 1;
            if (num >= enemiesToSpawn.Length)
            {
                num = 0;
            }
            return num;
        }
        else if (enemy <= 89)
        {
            num = 2;
            if (num >= enemiesToSpawn.Length)
            {
                num = 0;
            }
            return num;
        }
        else
        {
            num = 3;
            if (num >= enemiesToSpawn.Length)
            {
                num = 0;
            }
            return num;
        }
    }
}
