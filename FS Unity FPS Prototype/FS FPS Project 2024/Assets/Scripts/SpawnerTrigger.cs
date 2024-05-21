using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTrigger : MonoBehaviour
{
    [Header("---Array of enemies---")]
    [SerializeField] GameObject[] enemiesToSpawn;
    [Header("---Amount of enemies to spawn and delay between each spawn---")]
    [SerializeField] int numToSpawn;
    [SerializeField] int spawnDelay;
    [Header("---Weighting modifiers---(Adds to 100, 1 value is missing for the last array element, its just the remainder of the 100 minus the 5 numbers)")]
    [SerializeField] int Element0Weight;
    [SerializeField] int Element1Weight;
    [SerializeField] int Element2Weight;
    [SerializeField] int Element3Weight;
    [SerializeField] int Element4Weight;
    [SerializeField] int Element5Weight;
    [SerializeField] int Element6Weight;
    [SerializeField] int Element7Weight;
    [SerializeField] int Element8Weight;
    [Header("You can remove elements from the list, just make sure the weight on their corresponding element gets decreased, this gives you the ability to make the spawner spawn less than 6 enemies with the customizable weighting")]
    //private fields
    private int spawnCount;
    private bool isSpawning;
    private bool startSpawning;
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
        //Debug.Log(enemy);
        int num = 0;
        //spawn case 1
        if(enemy <= Element0Weight-1)
        {
            num = 0;
            return num;
        }
        //spawn case 2
        else if(enemy <= Element0Weight-1+Element1Weight)
        {
            num = 1;
            if (num >= enemiesToSpawn.Length)
            {
                num = 0;
            }
            return num;
        }
        //spawn case 3
        else if (enemy <= Element0Weight+Element1Weight+Element2Weight-1)
        {
            num = 2;
            if (num >= enemiesToSpawn.Length)
            {
                num = 0;
            }
            return num;
        }
        //spawn case 4
        else if (enemy <= Element0Weight+Element1Weight+Element2Weight+Element3Weight-1)
        {
            num = 3;
            if (num >= enemiesToSpawn.Length)
            {
                num = 0;
            }
            return num;
        }
        //spawn case 5
        else if (enemy <= Element0Weight + Element1Weight + Element2Weight + Element3Weight + Element4Weight -1)
        {
            num = 4;
            if (num >= enemiesToSpawn.Length)
            {
                num = 0;
            }
            return num;
        }
        //spawn case 6
        else if (enemy <= Element0Weight + Element1Weight + Element2Weight + Element3Weight + Element4Weight + Element5Weight - 1)
        {
            num = 5;
            if (num >= enemiesToSpawn.Length)
            {
                num = 0;
            }
            return num;
        }
        //spawn case 7
        else if (enemy <= Element0Weight + Element1Weight + Element2Weight + Element3Weight + Element4Weight + Element5Weight + Element6Weight - 1)
        {
            num = 6;
            if (num >= enemiesToSpawn.Length)
            {
                num = 0;
            }
            return num;
        }
        //spawn case 8
        else if (enemy <= Element0Weight + Element1Weight + Element2Weight + Element3Weight + Element4Weight + Element5Weight + Element6Weight + Element7Weight - 1)
        {
            num = 7;
            if (num >= enemiesToSpawn.Length)
            {
                num = 0;
            }
            return num;
        }
        //spawn case 9
        else
        {
            num = 8;
            if (num >= enemiesToSpawn.Length)
            {
                num = 0;
            }
            return num;
        }
    }
    public void resetSpawner()
    {
        spawnCount = 0;
    }
}
