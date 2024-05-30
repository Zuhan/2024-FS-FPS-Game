using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class MiniSkeleRedMageAI : MonoBehaviour, IDamage
{

    //Serialized fields for enemy ai
    [Header("----Main----")]
    [SerializeField] AudioSource Foot;
    [SerializeField] AudioSource HurtBody;
    [SerializeField] AudioSource attack;
    [SerializeField] AudioSource specialAbility;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer head;
    [SerializeField] Renderer torso;
    [SerializeField] Renderer arms;
    [SerializeField] Renderer legs;
    [SerializeField] Animator anim;
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] Component playerDetectiomRad;
    [SerializeField] Transform HeadPos;
    [SerializeField] GameObject Shield;
    [SerializeField] Image healthbar;
    [Header("----stats----")]
    [SerializeField] float shootRate;
    [SerializeField] float HP;
    [SerializeField] int pointsToGain;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int animSpeedTrans;
    [SerializeField] int viewCone;
    [SerializeField] float AttackRange;
    [SerializeField] float ShieldCooldown;
    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audHurt;
    [Range(0, 1)][SerializeField] float audVolHurt;
    [SerializeField] AudioClip[] audWalk;
    [Range(0, 1)][SerializeField] float audVolWalk;
    [SerializeField] AudioClip[] audAttack;
    [Range(0, 1)][SerializeField] float audVolAttack;
    [SerializeField] AudioClip[] audSpecialAbility;
    [Range(0, 1)][SerializeField] float audVolSpecialAbility;
    [SerializeField] float TimeBetweenSteps;


    bool playingWalk;
    float MaxHP;
    bool ShieldOn;
    bool ShieldOnCoolDown;
    float DamageAbsorb;
    float angleToPlayer;
    bool playerInRange;
    Vector3 playerDir;
    bool isShooting;
    Color enemycolor1;
    Color enemycolor2;
    Color enemycolor3;
    Color enemycolor4;
    private bool HurtOnCooldown;

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

        RaycastHit hit;

        if (Physics.Raycast(HeadPos.position, playerDir, out hit))
        {


            if (hit.collider.CompareTag("Player") && distanceToPlayer <= AttackRange && angleToPlayer <= viewCone)
            {
                if (!isShooting)
                    StartCoroutine(Shoot());

                if(ShieldOn == false && ShieldOnCoolDown == false)
                {
                    StartCoroutine(shieldSelf());
                }
            }
        }
    }


    public void attackSound()
    {
        attack.PlayOneShot(audAttack[UnityEngine.Random.Range(0, audAttack.Length)], audVolAttack);
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

        if(ShieldOn == true)
        {
            Shield.SetActive(false);
            DamageAbsorb += damage;
            damage = 0;
            ShieldOn = false;
        }
        else
        {
            HP -= damage;
            if (HurtOnCooldown == false)
            {
                StartCoroutine(PlayHurtAudio());
            }
        }

        UpdateEnemyUI();

        StartCoroutine(FlashRed());
        //set destination when damaged
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



    IEnumerator shieldSelf()
    {
        ShieldOnCoolDown = true;
        Shield.SetActive(true);
        ShieldOn = true;
        specialAbility.PlayOneShot(audSpecialAbility[UnityEngine.Random.Range(0, audSpecialAbility.Length)], audVolSpecialAbility);
        yield return new WaitForSeconds(ShieldCooldown);
        ShieldOnCoolDown = false;
    }

    IEnumerator WalkSound()
    {
        playingWalk = true;
        Foot.PlayOneShot(audWalk[UnityEngine.Random.Range(0, audWalk.Length)], audVolWalk);
        yield return new WaitForSeconds(TimeBetweenSteps);
        playingWalk = false;
    }


    IEnumerator PlayHurtAudio()
    {
        HurtOnCooldown = true;
        HurtBody.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audVolHurt);
        yield return new WaitForSeconds(audHurt.Length);
        HurtOnCooldown = false;
    }

    public void createBullet()
    {
        bullet.GetComponent<Bullet>().AddDamage(DamageAbsorb);
        Instantiate(bullet, shootPos.position, transform.rotation);
        bullet.GetComponent<Bullet>().RemoveDamage(DamageAbsorb);
        DamageAbsorb = 0;
    }

}
