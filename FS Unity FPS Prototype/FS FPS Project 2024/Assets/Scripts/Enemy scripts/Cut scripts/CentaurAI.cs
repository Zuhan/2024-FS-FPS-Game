using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CentaurAI : MonoBehaviour, IDamage
{

    //Serialized fields for enemy ai
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] int HP;
    [SerializeField] int pointsToGain;
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] Transform shootPos;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] Component playerDetectiomRad;
    [SerializeField] Component enemyCollider;
    [SerializeField] float timeBetweenBurstShot;
    [SerializeField] Light staffLight;
    [SerializeField] float enragedFireRate;
    [SerializeField] int hpAmountforEnraged;

    bool playerInRange;
    Vector3 playerDir;
    bool isShooting;
    Color enemycolor;

    // Start is called before the first frame update
    void Start()
    {
        //updates enemy count on start instance
        //gameManager.instance.updateGameGoal(1);
        //get starter enemy color
        enemycolor = model.material.color;
    }


    void Update()
    {

        if (playerInRange)
        {
            playerDir = gameManager.instance.player.transform.position - transform.position;
            agent.SetDestination(gameManager.instance.player.transform.position);


            //sets shoot rate to higher enraged shootrate if low hp
            if (shootRate != enragedFireRate && HP <= hpAmountforEnraged)
            {
                Enrage();
            }

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
        HP -= (int)damage;
        StartCoroutine(FlashRed());
        //set destination when damaged
        agent.SetDestination(gameManager.instance.player.transform.position);
        if (HP <= 0)
        {            
            Destroy(gameObject);

            gameManager.instance.pointsChange(pointsToGain);
            Debug.Log("Enemy died. Player gained " + pointsToGain + " points.");
        }
    }



    public void Enrage()
    {
        shootRate = enragedFireRate;
        this.agent.stoppingDistance = 10;
        staffLight.range = 3;
    }

    IEnumerator FlashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = enemycolor;
    }

    IEnumerator Shoot()
    {
        staffLight.enabled = false;
        isShooting = true;
        Instantiate(bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(timeBetweenBurstShot);
        Instantiate(bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(timeBetweenBurstShot);
        Instantiate(bullet, shootPos.position, transform.rotation);
        staffLight.enabled = true;
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

}
