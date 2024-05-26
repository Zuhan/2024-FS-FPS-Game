using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ArmoredSkele2HanderSummonAI : MonoBehaviour, IDamage
{

    //Serialized fields for enemy ai
    [Header ("----Main----")]
    [SerializeField] AudioSource Foot;
    [SerializeField] AudioSource HurtBody;
    [SerializeField] AudioSource Scythe;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] GameObject Helmet;
    [SerializeField] GameObject BracerArmL;
    [SerializeField] GameObject BracerArmR;
    [SerializeField] GameObject BracerLegL;
    [SerializeField] GameObject BracerLegR;
    [SerializeField] Animator anim;
    [SerializeField] Collider weaponCol;
    [SerializeField] Transform HeadPos;
    [SerializeField] Component playerDetectiomRad;
    [SerializeField] Image healthbar;
    [Header("----Stats----")]
    [SerializeField] float shootRate;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] float HP;
    [SerializeField] int pointsToGain;
    [SerializeField] int animSpeedTrans;
    [SerializeField] int viewCone;
    [SerializeField] float Armor;
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
    float TotalArmor;
    Color enemycolor1;
    List<GameObject> armorList;
    bool armorLReach1;
    bool armorLReach2;
    bool armorLReach3;
    bool armorLReach4;
    bool armorLReach5;

    // Start is called before the first frame update
    void Start()
    {
        SetUpEnemy();
    }


    void Update()
    {
        float animSpeed = agent.velocity.normalized.magnitude;
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), animSpeed, Time.deltaTime * animSpeedTrans));
        float distanceToPlayer = Vector3.Distance(transform.position, gameManager.instance.player.transform.position);

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

    private void SetUpEnemy()
    {
        //get starter enemy color
        enemycolor1 = model.material.color;
        TotalArmor = Armor;
        MaxHP = HP;
        armorList = new List<GameObject>
        {
            Helmet,
            BracerArmL,
            BracerArmR,
            BracerLegL,
            BracerLegR
        };
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
        //if armor amount is greater then 0
        if (Armor > 0)
        {
            //call armor reduction
            damage = ArmorReduction(damage); 
        }

       


        HP -= damage;
        HurtBody.PlayOneShot(audHurt[UnityEngine.Random.Range(0, audHurt.Length)], audVolHurt);
        StartCoroutine(FlashRed());
        //set destination when damaged

        UpdateEnemyUI();

        agent.SetDestination(gameManager.instance.player.transform.position);
        if (HP <= 0)
        {
            if (FindObjectOfType<NecromancerAI>().ShieldIsActive == true)
            {
                FindObjectOfType<NecromancerAI>().AddSkeleton(-1);
            }

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
        model.material.color = enemycolor1;
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
        Scythe.PlayOneShot(audAttack[UnityEngine.Random.Range(0, audAttack.Length)], audVolAttack);
    }


    private float ArmorReduction(float damage)
    {

       
        //generate a random int between 0 and half of the armor
        float randomArmor = UnityEngine.Random.Range(0, Armor / 2);

        if(Armor <= 2f)
        {
            randomArmor = Armor;
        }


        //subtract generated number from armor
        Armor -= randomArmor;
        


        damage -= randomArmor;

        //if damage is less than 0 
        if (damage < 0)
        {
            //subtract negative number from damage into armor
            Armor -= damage;
            //set damage to 0
            damage = 0f;
        }


        if (armorLReach5 != true)
        {
            if (armorLReach1 != true && Armor <= TotalArmor * .80f)
            {
                int ARindex = UnityEngine.Random.Range(0, armorList.Count);
                armorList[ARindex].SetActive(false);
                armorList.RemoveAt(ARindex);
                armorLReach1 = true;
                gameObject.GetComponent<NavMeshAgent>().speed += .30f;
                shootRate -= .25f;
            }
            else
            if (armorLReach2 != true && Armor <= TotalArmor * .60f)
            {
                int ARindex = UnityEngine.Random.Range(0, armorList.Count);
                armorList[ARindex].SetActive(false);
                armorList.RemoveAt(ARindex);
                armorLReach2 = true;
                gameObject.GetComponent<NavMeshAgent>().speed += .30f;
                shootRate -= .25f;
            }
            else
            if (armorLReach3 != true && Armor <= TotalArmor * .40f)
            {
                int ARindex = UnityEngine.Random.Range(0, armorList.Count);
                armorList[ARindex].SetActive(false);
                armorList.RemoveAt(ARindex);
                armorLReach3 = true;
                gameObject.GetComponent<NavMeshAgent>().speed += .30f;
                shootRate -= .25f;
            }
            else
            if (armorLReach4 != true && Armor <= TotalArmor * .20f)
            {
                int ARindex = UnityEngine.Random.Range(0, armorList.Count);
                armorList[ARindex].SetActive(false);
                armorList.RemoveAt(ARindex);
                armorLReach4 = true;
                gameObject.GetComponent<NavMeshAgent>().speed += .30f;
                shootRate -= .25f;
            }
            else
            if (armorLReach5 != true && Armor <= TotalArmor * .0f)
            {
                int ARindex = UnityEngine.Random.Range(0, armorList.Count);
                armorList[ARindex].SetActive(false);
                armorList.RemoveAt(ARindex);
                armorLReach5 = true;
                gameObject.GetComponent<NavMeshAgent>().speed += .30f;
                shootRate -= .25f;
            }
        }




        //return the reducted damage value
        return damage;
    }


    IEnumerator WalkSound()
    {
        playingWalk = true;
        Foot.PlayOneShot(audWalk[UnityEngine.Random.Range(0, audWalk.Length)], audVolWalk);
        yield return new WaitForSeconds(TimeBetweenSteps);
        playingWalk = false;
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
