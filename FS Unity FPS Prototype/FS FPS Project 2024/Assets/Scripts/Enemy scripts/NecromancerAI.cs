using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NecromancerAI : MonoBehaviour, IDamage
{

    //Serialized fields for enemy ai
    [Header("----Main----")]
    [SerializeField] AudioSource Summon;
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
    [SerializeField] Transform Spawn1;
    [SerializeField] Transform Spawn2;
    [SerializeField] Transform Spawn3;
    [SerializeField] Transform Spawn4;
    [SerializeField] Transform Spawn5;
    [SerializeField] GameObject SpawnType1;
    [SerializeField] GameObject SpawnType2;
    [SerializeField] GameObject SpawnType3;
    [SerializeField] GameObject Shield;
    [SerializeField] ParticleSystem PassPart;
    [Header("----Stats----")]  
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int animSpeedTrans;
    [SerializeField] float HP;  
    [SerializeField] float passiveshootRate;
    [SerializeField] int pointsToGain;
    [SerializeField] float SkeletonSummonDelay;
    [SerializeField] int SummonsTotal;
    [SerializeField] int DurOfPassive;
    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audHurt;
    [Range(0, 1)][SerializeField] float audVolHurt;
    [SerializeField] AudioClip[] audAttack;
    [Range(0, 1)][SerializeField] float audVolAttack;
    [SerializeField] AudioClip[] audSummon;
    [Range(0, 1)][SerializeField] float audVolSummon;

    //boss npc mode set to idle
    private NPCmode npcmode = NPCmode.Idle;

  
    bool OnPassiveCooldown;
    bool SummonOnCooldown;
    int SummonsAmountLeft;
    bool ShieldIsActive;
    float MaxHP;
    bool playerInRange;
    bool isShooting;
    Color enemycolor;
    List<Transform> SpawnList;
    List<GameObject> SpawnListType;

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
        SummonsAmountLeft = SummonsTotal;

        SpawnList = new List<Transform>(5)
        {
            Spawn1, Spawn2, Spawn3, Spawn4, Spawn5
        };

          SpawnListType = new List<GameObject>(3)
        {
            SpawnType1, SpawnType2, SpawnType3
        };
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
                    Barrier();
                }
                else
                {
                    //else set mode to summonskeletons
                    npcmode = NPCmode.SummonSkeletons;
                }
                break;
                //summon skeletons
                case NPCmode.SummonSkeletons:

                if (SummonsAmountLeft > 0)
                {
                    if (SummonOnCooldown == false)
                    {
                        SummonSkeletons();
                    }
                       
                }
                else
                {
                    //set to passive mode
                    //call cooldown on passive
                    WaitTimePassive();
                    npcmode = NPCmode.PassiveAttack;
                }
                break;
                //passive attack
                case NPCmode.PassiveAttack:
                   
                  if (OnPassiveCooldown == true)
                  {
                    PassPart.Play(true);
                    PassiveAttack();
                  }
                  else
                  {
                    //set shield to inactive
                    Shield.SetActive(false);
                    ShieldIsActive = false;
                    PassPart.Stop();
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

    public void TakeDamage(float damage)
    {
        if(ShieldIsActive == true) 
        {
            damage = 0;
        }


        HP -= damage;
        HurtBody.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audVolHurt);
        StartCoroutine(FlashRed());
        //set destination when damaged

        UpdateEnemyUI();

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

    IEnumerator PassiveShoot()
    {
        anim.SetTrigger("PassiveShoot");
        isShooting = true;
        yield return new WaitForSeconds(passiveshootRate);
        isShooting = false;
    }


    IEnumerator WaitTimePassive()
    {
        OnPassiveCooldown = true;
        yield return new WaitForSeconds(DurOfPassive);
        OnPassiveCooldown = false;
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

    private void Barrier()
    {
        Shield.SetActive(true);
        ShieldIsActive = true;
    }

    private void SummonSkeletons()
    {
        StartCoroutine(Summons());
    }

    private void AttackShadowBolts()
    {

    }

    private void Teleport()
    {

    }

    private void PassiveAttack()
    {
        if (isShooting == false)
        {
            StartCoroutine(PassiveShoot());
        }
    }
    //end of boss state functions

    IEnumerator Summons()
    {
        anim.SetTrigger("Summon");
        SummonOnCooldown = true;
        SummonsAmountLeft -= 1;
        //spawn random enemy at random location
        yield return new WaitForSeconds(SkeletonSummonDelay);
        SummonOnCooldown = false;
    }

    public void playSummonSound()
    {
        Summon.PlayOneShot(audSummon[Random.Range(0, audSummon.Length)], audVolSummon);
    }


    public void skeletonSum()
    {
        //spawn location
        int randomSpawn = Random.Range(0, 4);
        //type of enemy
        int randomType = Random.Range(0, 2);
        Instantiate(SpawnListType[randomType], SpawnList[randomSpawn].position, transform.rotation);
    }


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
