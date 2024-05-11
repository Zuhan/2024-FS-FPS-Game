using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : IBossState
{
    public IBossState DoState(BossSearch boss)
    {
        if(boss.agent == null)
        {
            boss.agent = boss.GetComponent<NavMeshAgent>();
        }

        switch(boss.hpValue)
        {
            case BossSearch.HPValue.aboveHalf:
                AttackPatternOne();
                break;
            case BossSearch.HPValue.halfOrBelow:
                AttackPatternTwo();
                break;
        }

        if (!boss.playerTarget.activeSelf)
        {
            return boss.idleState;
        }
        else
        {
            return boss.attackState;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void AttackPatternOne()
    {
        Debug.Log("Boss is above half HP");
    }

    private void AttackPatternTwo()
    {
        Debug.Log("Boss is below half HP");
    }    
}
