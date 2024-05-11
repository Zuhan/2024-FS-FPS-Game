using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : IBossState
{
    public IBossState DoState(BossSearch boss)
    {
        if (boss.agent == null)
        {
            boss.agent = boss.GetComponent<NavMeshAgent>();
        }

        if (PlayerInRange(boss))
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
    private bool PlayerInRange(BossSearch boss)
    {
        return false;
    }
}
