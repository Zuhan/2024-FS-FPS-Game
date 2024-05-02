using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BeholderAI : MonoBehaviour, IDamage
{

    //Serialized fields for enemy ai
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Animator anim;
    [SerializeField] int HP;
    [SerializeField] int pointsToGain;
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] Transform shootPos;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int animSpeedTrans;
    [SerializeField] Component playerDetectiomRad;
    [SerializeField] int viewCone;
    [SerializeField] Transform HeadPos;
    

    float angleToPlayer;
    bool playerInRange;
    Vector3 playerDir;
    bool isShooting;
    Color enemycolor;
    public waveSpawnerTwo spawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        //get starter enemy color
        enemycolor = model.material.color;
    }


    void Update()
    {
        float animSpeed = agent.velocity.normalized.magnitude;
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), animSpeed, Time.deltaTime * animSpeedTrans));

        if (playerInRange)
        {
            agent.SetDestination(gameManager.instance.player.transform.position);

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                faceTarget();

                canSeePlayer();
            }
           
        }
    }

    void canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - HeadPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, HeadPos.position.y + 1, playerDir.z), transform.forward);

        //Debug.Log(angleToPlayer);
        //Debug.DrawRay(HeadPos.position, playerDir);

        RaycastHit hit;

        if (Physics.Raycast(HeadPos.position, playerDir, out hit))
        {

            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewCone)
            {

                if (!isShooting)
                    StartCoroutine(Shoot());
              
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
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    // Take Damage AI added by Matt
    public void TakeDamage(int damage)
    {
        HP -= damage;
        StartCoroutine(FlashRed());
        //set destination when damaged
        agent.SetDestination(gameManager.instance.player.transform.position);
        if (HP <= 0)
        {            
            //removes a enemy from enemy count
            gameManager.instance.updateGameGoal(-1);
            if (spawnLocation)
            {
                spawnLocation.updateEnemyNumber();
            }
            Destroy(gameObject);
            //Points manager points add... (works? Sometimes?)
            PointsManager.Instance.AddPoints(pointsToGain);
            //Game manager points add... (Works, but not connected to player script)
            gameManager.instance.pointsChange(pointsToGain);
            //Debug.Log("Enemy died. Player gained " + pointsToGain + " points.");
        }
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
        anim.SetTrigger("Shoot");
       
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void createBullet()
    {
        Instantiate(bullet, shootPos.position, transform.rotation);
    }

}
