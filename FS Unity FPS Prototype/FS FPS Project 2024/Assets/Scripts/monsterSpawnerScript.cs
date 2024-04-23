using UnityEngine;
public class monsterSpawnerScript : MonoBehaviour
{
    public GameObject creatureSpawn;
    public float spawnRate = 2;
    public float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < spawnRate)
        {
            timer = timer + Time.deltaTime;
        }
        else
        {
            SpawnCreature();
            timer = 0;
        }
    }

    void SpawnCreature()
    {
        Instantiate(creatureSpawn, transform.position, transform.rotation);
    }
}
