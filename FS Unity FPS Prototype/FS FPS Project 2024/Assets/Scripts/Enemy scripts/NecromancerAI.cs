using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class NecromancerAI : MonoBehaviour, IDamage
{

    //Serialized fields for enemy ai
    [Header("----Main----")]
    [SerializeField] AudioSource Foot;
    [SerializeField] AudioSource HurtBody;
    [SerializeField] AudioSource attack;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Animator anim;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform HeadPos;
    [SerializeField] Transform shootPos;
    [SerializeField] Component playerDetectiomRad;
    [SerializeField] Image healthbar;
    [Header("----Stats----")]  
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int animSpeedTrans;
    [SerializeField] float HP;  
    [SerializeField] float shootRate;
    [SerializeField] int pointsToGain;
    [SerializeField] float AttackRange;
    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audHurt;
    [Range(0, 1)][SerializeField] float audVolHurt;
    [SerializeField] AudioClip[] audAttack;
    [Range(0, 1)][SerializeField] float audVolAttack;

    //boss npc mode set to idle
    private NPCmode npcmode = NPCmode.Idle;

    bool SkeletonsAreAlive;
    bool ShieldIsActive;
    float MaxHP;
    bool playerInRange;
    Vector3 playerDir;
    bool isShooting;
    Color enemycolor;

    // Start is called before the first frame update
    void Start()
    {
        //call set up enemy func
        SetUpEnemy();
    }


    //set up Enemy
    private void SetUpEnemy()
    {
        MaxHP = HP;
        UpdateEnemyUI();
        enemycolor = model.material.color;
    }


    void Update()
    {
        float animSpeed = agent.velocity.normalized.magnitude;
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), animSpeed, Time.deltaTime * animSpeedTrans));


        switch (npcmode)
        {

            case NPCmode.Idle:
                Idle();
                //if player in range from idle 
                if (playerInRange == true)
                {
                    //set mode to shield
                    npcmode = NPCmode.Shield;
                }
                //break from case
                break;

            case NPCmode.Shield:
                //if shield is not active
                if (ShieldIsActive == false)
                {
                    //call shield func
                    Shield();
                }
                else
                {
                    //else set mode to summonskeletons
                    npcmode = NPCmode.SummonSkeletons;
                }
                break;
                //summon skeletons
                case NPCmode.SummonSkeletons:

                if (SkeletonsAreAlive == false)
                {
                    SummonSkeletons();
                }
                else
                {
                  npcmode = NPCmode.PassiveAttack;
                }
                break;
                //passive attack
                case NPCmode.PassiveAttack:

                  if (SkeletonsAreAlive == false)
                  {
                    PassiveAttack();
                  }
                  else
                  {
                    npcmode = NPCmode.AttackShadowBolts;
                  }


                break;
                case NPCmode.AttackShadowBolts:
                   AttackShadowBolts();
                break;

                case NPCmode.Teleport:
                   Teleport();
                break;



            default:

            break;

        }
           
        
    }

    void canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - HeadPos.position;
        float distanceToPlayer = Vector3.Distance(transform.position, gameManager.instance.player.transform.position);

        RaycastHit hit;

        if (Physics.Raycast(HeadPos.position, playerDir, out hit))
        {

            if (hit.collider.CompareTag("Player") && distanceToPlayer <= AttackRange)
            {


                if (isShooting == false)
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

        //if shield is active damage is set to 0
        if (ShieldIsActive)
        {
            damage = 0;
        }

        HP -= damage;
        HurtBody.PlayOneShot(audHurt[UnityEngine.Random.Range(0, audHurt.Length)], audVolHurt);
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



    public void AttackSound()
    {
        attack.PlayOneShot(audAttack[Random.Range(0, audAttack.Length)], audVolAttack);
    }


    public void createBullet()
    {
        Instantiate(bullet, shootPos.position, transform.rotation);
    }
     

    //boss states start
    private void Idle()
    {

    }

    private void Shield()
    {
        ShieldIsActive = true;
    }

    private void SummonSkeletons()
    {

    }

    private void AttackShadowBolts()
    {

    }

    private void Teleport()
    {

    }

    private void PassiveAttack()
    {

    }
    //end of boss state functions




    //NPC enum boss states
    public enum NPCmode
    {
        Idle,
        SummonSkeletons,
        AttackShadowBolts,
        Shield,
        Teleport,
        PassiveAttack,
    }

}
