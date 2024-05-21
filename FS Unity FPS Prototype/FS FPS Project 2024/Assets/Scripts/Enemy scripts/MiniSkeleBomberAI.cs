using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MiniSkeleBomberAI : MonoBehaviour, IDamage
{
    [Header("----Main----")]
    //Serialized fields for enemy ai
    [SerializeField] AudioSource Foot;
    [SerializeField] AudioSource HurtBody;
    [SerializeField] AudioSource explosion;
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
    [SerializeField] Image healthbar;
    //[SerializeField] GameObject bullet;
    [Header("----stats----")]
    [SerializeField] float shootRate;
    [SerializeField] float HP;
    [SerializeField] int pointsToGain;
    //[SerializeField] Transform shootPos;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int animSpeedTrans;
    [SerializeField] int viewCone;
    [SerializeField] float AttackRange;
    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audHurt;
    [Range(0, 1)][SerializeField] float audVolHurt;
    [SerializeField] AudioClip[] audWalk;
    [Range(0, 1)][SerializeField] float audVolWalk;
    [SerializeField] float TimeBetweenSteps;


    bool playingWalk;
    float MaxHP;
    bool isDead;
    float angleToPlayer;
    bool playerInRange;
    Vector3 playerDir;
    bool isShooting;
    Color enemycolor1;
    Color enemycolor2;
    Color enemycolor3;
    Color enemycolor4;

    // Start is called before the first frame update
    void Start()
    {
        SetUpEnemy();
    }


    void Update()
    {
        float animSpeed = agent.velocity.normalized.magnitude;
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), animSpeed, Time.deltaTime * animSpeedTrans));

        if (playerInRange && isDead != true)
        {
            agent.SetDestination(gameManager.instance.player.transform.position);


            if (playingWalk == false && GetComponent<NavMeshAgent>().velocity.normalized.magnitude > 0.25f)
            {
                StartCoroutine(WalkSound());
            }

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
        enemycolor1 = head.material.color;
        enemycolor2 = torso.material.color;
        enemycolor3 = legs.material.color;
        enemycolor4 = arms.material.color;
    }

    void canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - HeadPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, HeadPos.position.y + 1, playerDir.z), transform.forward);
        float distanceToPlayer = Vector3.Distance(transform.position, gameManager.instance.player.transform.position);

       // Debug.Log(angleToPlayer);
       // Debug.DrawRay(HeadPos.position, playerDir);

        RaycastHit hit;

        if (Physics.Raycast(HeadPos.position, playerDir, out hit))
        {

            if ( hit.collider.CompareTag("Player") && distanceToPlayer <= AttackRange && angleToPlayer <= viewCone)
            {

                if (!isShooting)
                {
                    StartCoroutine(Shoot());
                }

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
        HurtBody.PlayOneShot(audHurt[UnityEngine.Random.Range(0, audHurt.Length)], audVolHurt);
        UpdateEnemyUI();
        StartCoroutine(FlashRed());
        //set destination when damaged
        agent.SetDestination(gameManager.instance.player.transform.position);
        if (HP <= 0)
        {
            isDead = true;
            HP = 100;
            UpdateEnemyUI();
            anim.SetTrigger("Shoot");
            //Game manager points add... (Works, but not connected to player script)
            gameManager.instance.pointsChange(pointsToGain);
            //Debug.Log("Enemy died. Player gained " + pointsToGain + " points.");
        }
    }

    IEnumerator WalkSound()
    {
        playingWalk = true;
        Foot.PlayOneShot(audWalk[UnityEngine.Random.Range(0, audWalk.Length)], audVolWalk);
        yield return new WaitForSeconds(TimeBetweenSteps);
        playingWalk = false;
    }


    void UpdateEnemyUI()
    {
        healthbar.fillAmount = HP / MaxHP;
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
        isDead = true;
        isShooting = true;
        anim.SetTrigger("Shoot");
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    IEnumerator BlowUp()
    {
        HP = 100;
        UpdateEnemyUI();
        //play the explosion
        explosion.Play();
        yield return new WaitForSeconds(.5f);
        //set VFX active and bomb radius to active
        VFX.SetActive(true);
        BombAOE.enabled = true;
        FlashRed();
        yield return new WaitForSeconds(1);
        //wait then destroy game object
        Destroy(gameObject);
    }
        



}
