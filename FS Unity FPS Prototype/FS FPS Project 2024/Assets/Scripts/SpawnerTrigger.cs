using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTrigger : MonoBehaviour
{
    [SerializeField] GameObject[] enemiesToSpawn;
    [SerializeField] int numToSpawn;
    [SerializeField] int spawnDelay;
    [Header("--- Weighting modifiers ---(Adds to 100, 1 value is missing for the mimic, its just the remainder of the 100 -  the 5 numbers)")]
    [SerializeField] int miniSkeleWeight1;
    [SerializeField] int miniSkeleWeight2;
    [SerializeField] int miniSkeleWeight3;
    [SerializeField] int beholderWeight;
    [SerializeField] int ArmoredSkeleWeight;
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
        //int enemy = Random.Range(0,enemiesToSpawn.Length);
        int enemy = enemyAlgorithim();
        Instantiate(enemiesToSpawn[enemy], transform.GetChild(spawnPos).position, transform.GetChild(spawnPos).rotation);
        spawnCount++;
        yield return new WaitForSeconds(spawnDelay);
        isSpawning=false;
    }

    //algorithim to spawn enemies in a weighted way to make enemies less likely or more likely to spawn
    //if enemies are removed from the list, it will fall back to the first enemy in the list as a default enemy to spawn
    private int enemyAlgorithim()
    {
        int enemy = Random.Range(0, 100);
        Debug.Log(enemy);
        int num = 0;
        //spawn case 1: mini skele
        if(enemy <= miniSkeleWeight1-1)
        {
            num = 0;
            return num;
        }
        //spawn case 2: skele red mage
        else if(enemy <= miniSkeleWeight2-1+miniSkeleWeight1)
        {
            num = 1;
            if (num >= enemiesToSpawn.Length)
            {
                num = 0;
            }
            return num;
        }
        //spawn case 3: skele yellow bomber
        else if (enemy <= miniSkeleWeight3+miniSkeleWeight2+miniSkeleWeight1-1)
        {
            num = 2;
            if (num >= enemiesToSpawn.Length)
            {
                num = 0;
            }
            return num;
        }
        //spawn case 4: beholder
        else if (enemy <= miniSkeleWeight1+miniSkeleWeight2+miniSkeleWeight3+beholderWeight-1)
        {
            num = 3;
            if (num >= enemiesToSpawn.Length)
            {
                num = 0;
            }
            return num;
        }
        //spawn case 5: armored melee skele
        else if (enemy <= miniSkeleWeight1 + miniSkeleWeight2 + miniSkeleWeight3 + beholderWeight + ArmoredSkeleWeight-1)
        {
            num = 4;
            if (num >= enemiesToSpawn.Length)
            {
                num = 0;
            }
            return num;
        }
        //spawn case 6: mimic
        else
        {
            num = 5;
            if (num >= enemiesToSpawn.Length)
            {
                num = 0;
            }
            return num;
        }
    }
}
