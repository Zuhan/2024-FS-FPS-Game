using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveSpawnerTwo : MonoBehaviour
{
    [Header("---Basic Enemy Stuff---")]
    [SerializeField] GameObject[] objectToSpawn;
    [SerializeField] int numToSpawn;
    [SerializeField] int spawnTimer;
    [SerializeField] Transform[] spawnPos;

    private int enemyIncrement = 0;

    int spawnCount;
    public bool isSpawning;
    public bool startSpawning = true;
    int numKilled;
    void Update()
    {
        if (startSpawning && !isSpawning && spawnCount < numToSpawn)
        {
            //Debug.Log("spawn being called");
            StartCoroutine(spawn());
        }
    }
    public void startWave(int multiplier)
    {
        //Debug.Log("spawn being called");
        startSpawning = setSpawnTrue.isSpawning;
        /*if (!startSpawning)
        {
            Debug.Log("notstartspawning");
        }
        else
        {
            startSpawning = true;
            Debug.Log("startSpawning");
            startSpawning = true;
        }*/
        isSpawning = false;
        /*if (!isSpawning)
        {
            Debug.Log("notisspawning");
        }
        else
        {
            Debug.Log("gigawrong");
        }*/
        spawnCount = 0;
        numToSpawn *= multiplier;
        gameManager.instance.updateGameGoal(numToSpawn);
        gameManager.instance.updateWave(waveManager.instance.waveCurrent);
    }
    IEnumerator spawn()
    {
        isSpawning = true;
        int arrayPos = Random.Range(0, spawnPos.Length);
        GameObject objectSpawned = Instantiate(objectToSpawn[Random.Range(0, objectToSpawn.Length)], spawnPos[arrayPos].position, spawnPos[arrayPos].rotation);
        if (objectSpawned.GetComponent<BeholderAI>())
        {
            objectSpawned.GetComponent<BeholderAI>().spawnLocation = this;
        }
        else if (objectSpawned.GetComponent<MimicAI>())
        {
            objectSpawned.GetComponent<MimicAI>().spawnLocation = this;
        }
        else if (objectSpawned.GetComponent<MiniSkeleAI>())
        {
            objectSpawned.GetComponent<MiniSkeleAI>().spawnLocation = this;
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
