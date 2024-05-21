using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MimicAI : MonoBehaviour, IDamage
{

    //Serialized fields for enemy ai
    [Header("----Main----")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Animator anim;
    [SerializeField] Collider weaponCol;
    //[SerializeField] GameObject bullet;
    //[SerializeField] Transform shootPos;
    [SerializeField] Component playerDetectiomRad;
    [SerializeField] Transform HeadPos;
    [SerializeField] Image healthbar;
    [Header("----Stats----")]
    [SerializeField] float shootRate;
    [SerializeField] float HP;
    [SerializeField] int pointsToGain;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int animSpeedTrans;
    [SerializeField] int viewCone;
    [SerializeField] float AttackRange;

    float MaxHP;
    float angleToPlayer;
    bool playerInRange;
    Vector3 playerDir;
    bool isShooting;
    Color enemycolor;

    // Start is called before the first frame update
    void Start()
    {
        SetUpEnemy();
    }


    void Update()
    {
        float animSpeed = agent.velocity.normalized.magnitude;
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), animSpeed, Time.deltaTime * animSpeedTrans));


        if (playerInRange)
        {
            agent.SetDestination(gameManager.instance.player.transform.position);

            healthbar.enabled = true;
         

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                faceTarget();

                canSeePlayer();
            }
           
        }
    }

    void SetUpEnemy()
    {
        MaxHP = HP;
        UpdateEnemyUI();
        //get starter enemy color
        enemycolor = model.material.color;
    }

    void canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - HeadPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, HeadPos.position.y + 1, playerDir.z), transform.forward);
        float distanceToPlayer = Vector3.Distance(transform.position, gameManager.instance.player.transform.position);

        //Debug.Log(angleToPlayer);
        //Debug.DrawRay(HeadPos.position, playerDir);

        RaycastHit hit;

        if (Physics.Raycast(HeadPos.position, playerDir, out hit))
        {

            if (hit.collider.CompareTag("Player") && distanceToPlayer <= AttackRange && angleToPlayer <= viewCone)
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
        UpdateEnemyUI();
        agent.SetDestination(gameManager.instance.player.transform.position);
        if (HP <= 0)
        {            
            Destroy(gameObject);
            //Game manager points add... (Works, but not connected to player script)
            gameManager.instance.pointsChange(pointsToGain);
            //Debug.Log("Enemy died. Player gained " + pointsToGain + " points.");
        }
    }

    void UpdateEnemyUI()
    {
        healthbar.fillAmount = HP / MaxHP;
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

    public void WeaponColOn()
    {
        weaponCol.enabled = true;
    }

    public void WeaponColOff()
    {
        weaponCol.enabled = false;
    }

}
