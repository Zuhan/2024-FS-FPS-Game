using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BeholderAI : MonoBehaviour, IDamage
{

    //Serialized fields for enemy ai
    [Header("----Main----")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Animator anim;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject Beam;
    [SerializeField] Transform HeadPos;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform BeamPos1;
    [SerializeField] Transform BeamPos2;
    [SerializeField] Transform BeamPos3;
    [SerializeField] Transform BeamPos4;
    [SerializeField] Transform BeamPos5;
    [SerializeField] Component playerDetectiomRad;
    [SerializeField] AudioSource aud;
    [SerializeField] Image healthbar;
    [Header("----Stats----")]  
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int animSpeedTrans;
    [SerializeField] float HP;  
    [SerializeField] float shootRate;
    [SerializeField] int pointsToGain;
    [SerializeField] int viewCone;
    [SerializeField] float AttackRange;
    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audBeam;
    [Range(0, 1)][SerializeField] float audBeamVol;


    float MaxHP;
    bool LowHpReached = false;
    float angleToPlayer;
    bool playerInRange;
    Vector3 playerDir;
    bool isShooting;
    Color enemycolor;
    List<Transform> BeamList;

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
        BeamList = new List<Transform>(5);
        enemycolor = model.material.color;
        BeamList.Add(BeamPos1);
        BeamList.Add(BeamPos2);
        BeamList.Add(BeamPos3);
        BeamList.Add(BeamPos4);
        BeamList.Add(BeamPos5);
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
        float distanceToPlayer = Vector3.Distance(transform.position, gameManager.instance.player.transform.position);

        //Debug.Log(angleToPlayer);
        //Debug.DrawRay(HeadPos.position, playerDir);

        RaycastHit hit;

        if (Physics.Raycast(HeadPos.position, playerDir, out hit))
        {

            if (hit.collider.CompareTag("Player") && distanceToPlayer <= AttackRange && angleToPlayer <= viewCone)
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
        HP -= damage;
        StartCoroutine(FlashRed());
        //set destination when damaged

        UpdateEnemyUI();

        if (LowHpReached == false && HP <= MaxHP / 2)
        {
            LowHpAttack();
        }

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

    //low hp attack func
    public void LowHpAttack()
    {
        isShooting = true;
        anim.SetTrigger("BeamAttack");
        aud.PlayOneShot(audBeam[Random.Range(0, audBeam.Length)], audBeamVol);
        LowHpReached = true;
        isShooting = false;
    }


    //shoot beam func for low hp attack in anim
    public void ShootBeam()
    {
       int random = UnityEngine.Random.Range(0, 4);
       Instantiate(Beam, BeamList[random].position, transform.rotation);
    }

    public void createBullet()
    {
        Instantiate(bullet, shootPos.position, transform.rotation);
    }

}
