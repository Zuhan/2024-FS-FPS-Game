using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class mimicAI : MonoBehaviour, IDamage
{

    //Serialized fields for enemy ai
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] int HP;
    [SerializeField] int pointsToGain;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] Component playerDetectiomRad;
    [SerializeField] Component enemyCollider;
    [SerializeField] Collider weaponCol;


    bool playerInRange;
    Vector3 playerDir;
    Color enemycolor;
    float angleToPlayer;
    

    // Start is called before the first frame update
    void Start()
    {
        //updates enemy count on start instance
        gameManager.instance.updateGameGoal(1);
        //get starter enemy color
        enemycolor = model.material.color;
    }


    void Update()
    {

        if (playerInRange)
        {
            playerDir = gameManager.instance.player.transform.position - transform.position;
            angleToPlayer = Vector3.Angle(playerDir, transform.forward);

            agent.SetDestination(gameManager.instance.player.transform.position);


            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                faceTarget();
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
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    // Take Damage AI added by Matt
    public void TakeDamage(int damage)
    {
        HP -= damage;
        StartCoroutine(FlashRed());
        //set destination when damaged
        agent.SetDestination(gameManager.instance.player.transform.position);
        if (HP <= 0)
        {            
            //removes a enemy from enemy count
            gameManager.instance.updateGameGoal(-1);
            
            Destroy(gameObject);
            //Points manager points add... (works? Sometimes?)
            PointsManager.Instance.AddPoints(pointsToGain);
            //Game manager points add... (Works, but not connected to player script)
            gameManager.instance.pointsChange(pointsToGain);
            Debug.Log("Enemy died. Player gained " + pointsToGain + " points.");
        }
    }
    IEnumerator FlashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = enemycolor;
    }


    public void weaponColOn()
    {
        weaponCol.enabled = true;
    }


    public void weaponColOff()
    {
        weaponCol.enabled = false;
    }
}