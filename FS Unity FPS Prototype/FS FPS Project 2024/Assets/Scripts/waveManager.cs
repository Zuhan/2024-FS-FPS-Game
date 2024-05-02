using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveManager : MonoBehaviour
{
    public static waveManager instance;
    public waveSpawnerTwo[] spawners;
    [SerializeField] int timeBetweenWaves;
    [SerializeField] int enemyIncrement;
    public int waveCurrent;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        StartCoroutine(startWave());
    }

    public IEnumerator startWave()
    {
        waveCurrent++;
        if (waveCurrent <= spawners.Length)
        {
            yield return new WaitForSeconds(timeBetweenWaves);
            spawners[waveCurrent - 1].startWave(enemyIncrement);
            enemyIncrement++;
        }
    }
}
