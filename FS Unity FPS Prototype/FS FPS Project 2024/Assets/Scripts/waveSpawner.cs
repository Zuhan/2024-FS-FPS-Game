using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class waveSpawner : MonoBehaviour
{
    [Header("---Basic Enemy Stuff---")]
    [SerializeField] GameObject[] objectToSpawn;
    [SerializeField] int numToSpawn;
    [SerializeField] int spawnTimer;
    [SerializeField] Transform[] spawnPos;
    
    private int enemyIncrement = 0;

    int spawnCount;
    bool isSpawning;
    bool startSpawning;
    int numKilled;
    void Update()
    {
        if (startSpawning && !isSpawning && spawnCount < numToSpawn)
        {
            StartCoroutine(spawn());
        }
    }
    public void startWave(int multiplier)
    {
        startSpawning = setSpawnTrue.isSpawning;
        numToSpawn *= multiplier;
        gameManager.instance.updateWave(waveManager.instance.waveCurrent);
    }
    IEnumerator spawn()
    {
        Debug.Log("spawn called");
        isSpawning = true;
        int arrayPos = Random.Range(0, spawnPos.Length);
        GameObject objectSpawned = Instantiate(objectToSpawn[Random.Range(0,objectToSpawn.Length)], spawnPos[arrayPos].position, spawnPos[arrayPos].rotation);
        if (objectSpawned.GetComponent<BeholderAI>())
        {
            //objectSpawned.GetComponent<BeholderAI>().spawnLocation = this;
        }
        else if (objectSpawned.GetComponent<MimicAI>())
        {
            //objectSpawned.GetComponent<MimicAI>().spawnLocation = this;
        }
        else if (objectSpawned.GetComponent<MiniSkeleAI>())
        {
            //objectSpawned.GetComponent <SkeleAI>().spawnLocation = this;
        }
        spawnCount++;
        yield return new WaitForSeconds(spawnTimer);
        isSpawning = false;
    }
    public void updateEnemyNumber()
    {
        numKilled++;
        if (numKilled >= numToSpawn)
        {
            startSpawning = false;
            StartCoroutine(waveManager.instance.startWave());
        }
    }
}
