using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BossSearch : MonoBehaviour, IDamage
{
    [SerializeField] private string stateName;
    [SerializeField] private IBossState currentState;
    [SerializeField] public SphereCollider trigger;

    [SerializeField] public GameObject BossCenterPOS;
    [SerializeField] public GameObject BossRightPOS;
    [SerializeField] public GameObject BossLeftPOS;
    [SerializeField] public GameObject FarRightPOS;
    [SerializeField] public GameObject FarLeftPOS;

    [SerializeField] public GameObject UICenter;
    [SerializeField] public GameObject UIBossRight;
    [SerializeField] public GameObject UIBossLeft;
    [SerializeField] public GameObject UIFarRight;
    [SerializeField] public GameObject UIFarLeft;

    [SerializeField] public GameObject ExplosionCenter;
    [SerializeField] public GameObject ExplosionBossRight;
    [SerializeField] public GameObject ExplosionBossLeft;
    [SerializeField] public GameObject ExplosionFarRight;
    [SerializeField] public GameObject ExplosionFarLeft;

    [SerializeField] public SphereCollider CenterTrigger;
    [SerializeField] public SphereCollider BossRightTrigger;
    [SerializeField] public SphereCollider BossLeftTrigger;
    [SerializeField] public SphereCollider FarRightTrigger;
    [SerializeField] public SphereCollider FarLeftTrigger;

    [SerializeField] public GameObject Card_TheWorld;
    [SerializeField] public GameObject Card_TheMagician;
    [SerializeField] public GameObject Card_Justice;
    [SerializeField] public GameObject Card_TheTower;

    [SerializeField] public Renderer body;
    [SerializeField] public Renderer RArm;
    [SerializeField] public Renderer LArm;

    [SerializeField] public CapsuleCollider bodyHitBox;
    [SerializeField] public CapsuleCollider LArmHitBox;
    [SerializeField] public CapsuleCollider RArmHitBox;

    public List<GameObject> cardDeck = new List<GameObject>();

    public IdleState idleState = new IdleState();
    public AttackState attackState = new AttackState();

    public NavMeshAgent agent;

    public Vector3 nextDashLocation;
    public GameObject playerTarget;

    public float HP;
    public float maxHP;

    public int pointsToGain = 2500;

    public bool playerInRange = false;

    public HPValue hpValue = new HPValue();



    // Start is called before the first frame update
    void OnEnable()
    {
        HP = maxHP;
        currentState = idleState;
        trigger.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (playerInRange)
            {
                Debug.Log("Player is in range");
            }
            playerTarget = other.gameObject;
        }
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        StartCoroutine(FlashRed());

        if(HP <= 0)
        {
            Destroy(gameObject);

            PointsManager.Instance.AddPoints(pointsToGain);

            gameManager.instance.pointsChange(pointsToGain);
        }

    }

    IEnumerator FlashRed()
    {
        body.material.color = Color.red;
        RArm.material.color = Color.red;
        LArm.material.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        body.material.color = Color.white;
        RArm.material.color = Color.white;
        LArm.material.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.DoState(this);
        stateName = currentState.ToString();

        if(HP <= (maxHP / 4))
        {
            hpValue = HPValue.quarterOrBelow;
        }
        else
        {
            hpValue = HPValue.highHP;
        }
    }
    public enum HPValue
    {
        highHP,
        quarterOrBelow
    }
}
