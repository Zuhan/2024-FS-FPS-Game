using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveSpawner : MonoBehaviour
{

    [SerializeField] GameObject objectToSpawn;
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
        GameObject objectSpawned = Instantiate(objectToSpawn, spawnPos[arrayPos].position, spawnPos[arrayPos].rotation);
        if (objectSpawned.GetComponent<BeholderAI>())
        {
            objectSpawned.GetComponent<BeholderAI>().spawnLocation = this;
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
