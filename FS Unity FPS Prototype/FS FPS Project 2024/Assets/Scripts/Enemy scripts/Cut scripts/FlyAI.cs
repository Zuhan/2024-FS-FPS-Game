using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class flyAI : MonoBehaviour, IDamage
{

    //Serialized fields for enemy ai
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] float HP;
    [SerializeField] int pointsToGain;
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] float wingSpeed;
    [SerializeField] Transform shootPos;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] Component playerDetectiomRad;
    [SerializeField] Component enemyCollider;
    [SerializeField] GameObject lWingTop;
    [SerializeField] GameObject lWingMiddle;
    [SerializeField] GameObject lWingBot;
    [SerializeField] GameObject rWingTop;
    [SerializeField] GameObject rWingMiddle;
    [SerializeField] GameObject rWingBottom;
    [SerializeField] Transform flySpawn1;
    [SerializeField] Transform flySpawn2;
    [SerializeField] GameObject enemySpawned;



    bool spawnedFlys;
    bool playerInRange;
    Vector3 playerDir;
    bool isShooting;
    Color enemycolor;
    bool hasFlapped;
    

    // Start is called before the first frame update
    void Start()
    {
        //updates enemy count on start instance
        gameManager.instance.updateGameGoal(1);
        //get starter enemy color
        enemycolor = model.material.color;
        //set spawn flys to false
        spawnedFlys = false;
    }


    void Update()
    {
        if(hasFlapped == false)
        {
            StartCoroutine(FlapWings());
        }

        //if flys were not spawned and hp is 5 or less
        if (spawnedFlys == false && HP <= 5)
        {
            FlySpawn();
        }


        if (playerInRange)
        {
            playerDir = gameManager.instance.player.transform.position - transform.position;
            agent.SetDestination(gameManager.instance.player.transform.position);

            if (!isShooting)
                StartCoroutine(Shoot());

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                faceTarget();
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    // Take Damage AI added by Matt
    public void TakeDamage(float damage)
    {
        HP -= damage;
        StartCoroutine(FlashRed());
        //set destination when damaged
        agent.SetDestination(gameManager.instance.player.transform.position);
        if (HP <= 0)
        {            
            //removes a enemy from enemy count
            gameManager.instance.updateGameGoal(-1);
            
            Destroy(gameObject);
            //Points manager points add... (works? Sometimes?)
            PointsManager.Instance.AddPoints(pointsToGain);
            //Game manager points add... (Works, but not connected to player script)
            gameManager.instance.pointsChange(pointsToGain);
            Debug.Log("Enemy died. Player gained " + pointsToGain + " points.");
        }
    }


    public void FlySpawn()
    {
        Instantiate(enemySpawned, flySpawn1.position, Quaternion.LookRotation(playerDir));
        Instantiate(enemySpawned, flySpawn2.position, Quaternion.LookRotation(playerDir));
        spawnedFlys = true;
    }

    IEnumerator FlapWings()
    {
        hasFlapped = true;
        lWingTop.SetActive(!lWingTop.activeSelf);
        lWingMiddle.SetActive(!lWingMiddle.activeSelf);
        lWingBot.SetActive(!lWingBot.activeSelf);
        rWingTop.SetActive(!rWingTop.activeSelf);
        rWingMiddle.SetActive(!rWingMiddle.activeSelf);
        rWingBottom.SetActive(!rWingBottom.activeSelf);
        yield return new WaitForSeconds(wingSpeed);
        hasFlapped = false;
    }

    IEnumerator FlashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = enemycolor;
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        Instantiate(bullet, shootPos.position, transform.rotation);
       yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

}