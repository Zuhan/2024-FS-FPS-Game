using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ArmoredSkeleAI : MonoBehaviour, IDamage
{

    //Serialized fields for enemy ai
    [Header ("----Main----")]
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
    //[SerializeField] GameObject bullet;
    //[SerializeField] Transform shootPos;
    [Header("----Stats----")]
    [SerializeField] float shootRate;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] float HP;
    [SerializeField] int pointsToGain;
    [SerializeField] int animSpeedTrans;
    [SerializeField] int viewCone;
    [SerializeField] float Armor;

    float TotalHP;
    float angleToPlayer;
    bool playerInRange;
    Vector3 playerDir;
    bool isShooting;
    float TotalArmor;
    Color enemycolor1;
    public waveSpawnerTwo spawnLocation;
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

    private void SetUpEnemy()
    {
        //get starter enemy color
        enemycolor1 = model.material.color;
        TotalArmor = Armor;
        TotalHP = HP;
        armorList = new List<GameObject>
        {
            Helmet,
            BracerArmL,
            BracerArmR,
            BracerLegL,
            BracerLegR
        };
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
        //if armor amount is greater then 0
        if (Armor > 0)
        {
            //call armor reduction
            damage = ArmorReduction(damage); 
        }


        HP -= damage;
        StartCoroutine(FlashRed());
        //set destination when damaged
        agent.SetDestination(gameManager.instance.player.transform.position);
        if (HP <= 0)
        {            
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
        model.material.color = enemycolor1;
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        anim.SetTrigger("Shoot");
       
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
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
            }
            else
            if (armorLReach2 != true && Armor <= TotalArmor * .60f)
            {
                int ARindex = UnityEngine.Random.Range(0, armorList.Count);
                armorList[ARindex].SetActive(false);
                armorList.RemoveAt(ARindex);
                armorLReach2 = true;
                gameObject.GetComponent<NavMeshAgent>().speed += .30f;
            }
            else
            if (armorLReach3 != true && Armor <= TotalArmor * .40f)
            {
                int ARindex = UnityEngine.Random.Range(0, armorList.Count);
                armorList[ARindex].SetActive(false);
                armorList.RemoveAt(ARindex);
                armorLReach3 = true;
                gameObject.GetComponent<NavMeshAgent>().speed += .30f;
            }
            else
            if (armorLReach4 != true && Armor <= TotalArmor * .20f)
            {
                int ARindex = UnityEngine.Random.Range(0, armorList.Count);
                armorList[ARindex].SetActive(false);
                armorList.RemoveAt(ARindex);
                armorLReach4 = true;
                gameObject.GetComponent<NavMeshAgent>().speed += .30f;
            }
            else
            if (armorLReach5 != true && Armor <= TotalArmor * .0f)
            {
                int ARindex = UnityEngine.Random.Range(0, armorList.Count);
                armorList[ARindex].SetActive(false);
                armorList.RemoveAt(ARindex);
                armorLReach5 = true;
                gameObject.GetComponent<NavMeshAgent>().speed += .30f;
            }
        }




        //return the reducted damage value
        return damage;
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
