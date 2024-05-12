using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MiniSkeleBomberAI : MonoBehaviour, IDamage
{
    [Header("----Main----")]
    //Serialized fields for enemy ai
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer head;
    [SerializeField] Renderer torso;
    [SerializeField] Renderer arms;
    [SerializeField] Renderer legs;
    [SerializeField] Animator anim;
    [SerializeField] Transform HeadPos;
    [SerializeField] Component playerDetectiomRad;
    [SerializeField] Collider BombAOE;
    [SerializeField] GameObject VFX;
    [SerializeField] AudioSource explosion;
    //[SerializeField] GameObject bullet;
    [Header("----stats----")]
    [SerializeField] float shootRate;
    [SerializeField] float HP;
    [SerializeField] int pointsToGain;
    //[SerializeField] Transform shootPos;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int animSpeedTrans;
    [SerializeField] int viewCone;



    float angleToPlayer;
    bool playerInRange;
    Vector3 playerDir;
    bool isShooting;
    Color enemycolor1;
    Color enemycolor2;
    Color enemycolor3;
    Color enemycolor4;
    public waveSpawnerTwo spawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        //get starter enemy color
        enemycolor1 = head.material.color;
        enemycolor2 = torso.material.color;
        enemycolor3 = legs.material.color;
        enemycolor4 = arms.material.color;
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
    public void TakeDamage(float damage)
    {
        HP -= damage;
        StartCoroutine(FlashRed());
        //set destination when damaged
        agent.SetDestination(gameManager.instance.player.transform.position);
        if (HP <= 0)
        {
            anim.SetTrigger("Shoot");
            //Points manager points add... (works? Sometimes?)
            PointsManager.Instance.AddPoints(pointsToGain);
            //Game manager points add... (Works, but not connected to player script)
            gameManager.instance.pointsChange(pointsToGain);
            //Debug.Log("Enemy died. Player gained " + pointsToGain + " points.");
        }
    }
    IEnumerator FlashRed()
    {
        head.material.color = Color.red;
        torso.material.color = Color.red;
        legs.material.color = Color.red;
        arms.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        head.material.color = enemycolor1;
        torso.material.color = enemycolor2;
        legs.material.color = enemycolor3;
        arms.material.color = enemycolor4;
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        anim.SetTrigger("Shoot");

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    IEnumerator BlowUp()
    {
        //play the explosion
        explosion.Play();
        yield return new WaitForSeconds(.5f);
        HP = 100;
        //set speed and rotate speed to 0
        GetComponent<NavMeshAgent>().speed = 0;
        faceTargetSpeed = 0;
        //set VFX active and bomb radius to active
        VFX.SetActive(true);
        BombAOE.enabled = true;
        FlashRed();
        yield return new WaitForSeconds(1);
        //wait then destroy game object
        Destroy(gameObject);
    }
        



}
