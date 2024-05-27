using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : IBossState
{
    int sayThiOnce = 0;
    public IBossState DoState(BossSearch boss)
    {
        while(sayThiOnce == 0)
        {
            sayThiOnce++;
        }

        if (boss.agent == null)
        {
            boss.agent = boss.GetComponent<NavMeshAgent>();
        }

        if (boss.playerInRange)
        {
            return boss.attackState;
        }
        else
        { return boss.idleState; }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
