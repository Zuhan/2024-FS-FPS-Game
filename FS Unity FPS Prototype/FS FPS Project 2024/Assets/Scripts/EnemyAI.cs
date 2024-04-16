using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{

    //Serialized fields for enemy ai
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] int HP;
    [SerializeField] int pointsToGain;
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] Transform shootPos;

    bool isShooting;
    

    // Start is called before the first frame update
    void Start()
    {
        //updates enemy count on start instance
        gameManager.instance.updateGameGoal(1);
    }

    void Update()
    {
        //set destination to player location
        agent.SetDestination(gameManager.instance.player.transform.position);

        if (!isShooting)
            StartCoroutine(Shoot());
    }

    // Take Damage AI added by Matt

    public void TakeDamage(int damage)
    {
        HP -= damage;
        StartCoroutine(FlashRed());
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
        model.material.color = Color.white;
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        Instantiate(bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

}
