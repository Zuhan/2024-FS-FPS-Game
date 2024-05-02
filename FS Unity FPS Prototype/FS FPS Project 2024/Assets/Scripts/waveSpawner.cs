using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveSpawner : MonoBehaviour
{

    [SerializeField] GameObject[] objectToSpawn;
    [SerializeField] int numToSpawn;
    [SerializeField] int spawnTimer;
    [SerializeField] Transform[] spawnPos;

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
    public void startWave()
    {
        startSpawning = true;
        gameManager.instance.updateGameGoal(numToSpawn);
        gameManager.instance.updateWave(waveManager.instance.waveCurrent);
    }
    IEnumerator spawn()
    {
        isSpawning = true;
        int arrayPos = Random.Range(0, spawnPos.Length);
        GameObject objectSpawned = Instantiate(objectToSpawn[Random.Range(0,objectToSpawn.Length)], spawnPos[arrayPos].position, spawnPos[arrayPos].rotation);
        if (objectSpawned.GetComponent<BeholderAI>())
        {
            objectSpawned.GetComponent<BeholderAI>().spawnLocation = this;
        }
        else if (objectSpawned.GetComponent<MimicAI>())
        {
            objectSpawned.GetComponent<MimicAI>().spawnLocation = this;
        }
        else if (objectSpawned.GetComponent<CentaurAI>())
        {
            objectSpawned.GetComponent <CentaurAI>().spawnLocation = this;
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
