using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BossSearch : MonoBehaviour
{
    [SerializeField] private string stateName;
    [SerializeField] private IBossState currentState;
    [SerializeField] public SphereCollider trigger;

    public IdleState idleState = new IdleState();
    public AttackState attackState = new AttackState();

    public NavMeshAgent agent;

    public Vector3 nextDashLocation;
    public GameObject playerTarget;

    public int HP;
    public int maxHP;

    public HPValue hpValue = new HPValue();



    // Start is called before the first frame update
    void OnEnable()
    {
        currentState = idleState;
        trigger.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.DoState(this);
        stateName = currentState.ToString();

        if(HP <= (maxHP / 2))
        {
            hpValue = HPValue.halfOrBelow;
        }
        else
        {
            hpValue = HPValue.aboveHalf;
        }
    }
    public enum HPValue
    {
        aboveHalf,
        halfOrBelow
    }
}
