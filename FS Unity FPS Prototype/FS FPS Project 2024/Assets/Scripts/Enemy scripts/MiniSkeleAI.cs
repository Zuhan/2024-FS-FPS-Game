using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MiniSkeleAI : MonoBehaviour, IDamage
{

    //Serialized fields for enemy ai
    [Header("----Main----")]
    [SerializeField] AudioSource Foot;
    [SerializeField] AudioSource HurtBody;
    [SerializeField] AudioSource Sword;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer head;
    [SerializeField] Renderer torso;
    [SerializeField] Renderer arms;
    [SerializeField] Renderer legs;
    [SerializeField] Animator anim;
    [SerializeField] Collider weaponCol;
    [SerializeField] Component playerDetectiomRad;
    [SerializeField] Transform HeadPos;
    [SerializeField] Image healthbar;
    [Header("----Stats----")]
    [SerializeField] float shootRate;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int animSpeedTrans;
    [SerializeField] float HP;
    [SerializeField] int pointsToGain;
    [SerializeField] int viewCone;
    [SerializeField] float AttackRange;
    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audHurt;
    [Range(0, 1)][SerializeField] float audVolHurt;
    [SerializeField] AudioClip[] audWalk;
    [Range(0, 1)][SerializeField] float audVolWalk;
    [SerializeField] AudioClip[] audAttack;
    [Range(0, 1)][SerializeField] float audVolAttack;
    [SerializeField] float TimeBetweenSteps;


    bool playingWalk;
    float MaxHP;
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
        //get starter enemy color
        enemycolor1 = head.material.color;
        enemycolor2 = torso.material.color;
        enemycolor3 = legs.material.color;
        enemycolor4 = arms.material.color;
        UpdateEnemyUI();
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
        if (HurtOnCooldown == false)
        {
            StartCoroutine(PlayHurtAudio());
        }
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


    public void attackSound()
    {
        Sword.PlayOneShot(audAttack[UnityEngine.Random.Range(0, audAttack.Length)], audVolAttack);
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

    public void WeaponColOn()
    {
        weaponCol.enabled = true;
    }

    public void WeaponColOff()
    {
        weaponCol.enabled = false;
    }

}
