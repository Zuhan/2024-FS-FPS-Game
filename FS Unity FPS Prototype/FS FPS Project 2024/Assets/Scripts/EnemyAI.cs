using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    //Seralized fields for enemy ai
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] int HP;


   // Start is called before the first frame update
    void Start()
    {
       

    }

    void Update()
    {
        //set destitnation to player location
        agent.SetDestination(gameManager.Instance.player.transform.position);
    }
}
