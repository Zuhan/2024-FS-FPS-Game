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

    [Header("-----The World-----")]
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

    [Header("-----Cards-----")]
    [SerializeField] public GameObject Card_TheWorld;
    [SerializeField] public GameObject Card_TheMagician;
    [SerializeField] public GameObject Card_Justice;
    [SerializeField] public GameObject Card_TheTower;

    [Header("-----Justice-----")]
    [Header("-----Body-----")]

    [SerializeField] public Renderer button;
    [SerializeField] public Renderer cheek;
    [SerializeField] public Renderer hair_acc;
    [SerializeField] public Renderer hair_front;
    [SerializeField] public Renderer hair_side;
    [SerializeField] public Renderer hairband;
    [SerializeField] public Renderer leg;
    [SerializeField] public Renderer shirts;
    [SerializeField] public Renderer shirts_s;
    [SerializeField] public Renderer shirts_bk;
    [SerializeField] public Renderer skin;
    [SerializeField] public Renderer tail;
    [SerializeField] public Renderer tail_bottom;
    [SerializeField] public Renderer uwagi;
    [SerializeField] public Renderer uwagi_bk;

    [SerializeField] public Renderer buttonHB;
    [SerializeField] public Renderer cheekHB;
    [SerializeField] public Renderer hair_accHB;
    [SerializeField] public Renderer hair_frontHB;
    [SerializeField] public Renderer hair_sideHB;
    [SerializeField] public Renderer hairbandHB;
    [SerializeField] public Renderer legHB;
    [SerializeField] public Renderer shirtsHB;
    [SerializeField] public Renderer shirts_sHB;
    [SerializeField] public Renderer shirts_bkHB;
    [SerializeField] public Renderer skinHB;
    [SerializeField] public Renderer tailHB;
    [SerializeField] public Renderer tail_bottomHB;
    [SerializeField] public Renderer uwagiHB;
    [SerializeField] public Renderer uwagi_bkHB;


    [SerializeField] public GameObject justiceObj;
    [SerializeField] public SpawnerTrigger justiceTrigger;

    [Header("-----The Magician-----")]
    [SerializeField] public GameObject aura1;
    [SerializeField] public GameObject aura2;
    [SerializeField] public GameObject aura3;
    [SerializeField] public GameObject aura4;
    [SerializeField] public GameObject aura5;

    [SerializeField] public GameObject auraUnderBoss;

    [SerializeField] public GameObject magiShootPOS1;
    [SerializeField] public GameObject magiShootPOS2;
    [SerializeField] public GameObject magiShootPOS3;
    [SerializeField] public GameObject magiShootPOS4;
    [SerializeField] public GameObject magiShootPOS5;

    [SerializeField] public GameObject bullet;
    [SerializeField] public float projDmg;


    [SerializeField] public Rigidbody rb;
    [SerializeField] public GameObject magicianProjectile;
    
    [SerializeField] public GameObject shootPos;
    public Vector3 playerDir;


    [Header("-----Miscellaneous-----")]
    public List<GameObject> auraList = new List<GameObject>();
    public List<GameObject> shootPosList = new List<GameObject>();
    public List<GameObject> cardDeck = new List<GameObject>();

    public IdleState idleState = new IdleState();
    public AttackState attackState = new AttackState();

    public NavMeshAgent agent;

    public Vector3 nextDashLocation;
    public GameObject playerTarget;

    public float HP;
    public float maxHP;

    public bool playerInRange = false;
    public bool castingSpell = false;

    public HPValue hpValue = new HPValue();


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

    public void InstantiateBullet(Vector3 pos)
    {
        Instantiate(magicianProjectile, pos, transform.rotation);
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        StartCoroutine(FlashRed());

        if(HP <= 0)
        {
            Destroy(gameObject);

            gameManager.instance.win();
        }

    }

    IEnumerator FlashRed()
    {
        Color buttonColor = button.material.color;
        Color cheekColor = cheek.material.color;
        Color hair_accColor = hair_acc.material.color;
        Color hair_frontColor = hair_front.material.color;
        Color hair_sideColor = hair_side.material.color;
        Color hairbandColor = hairband.material.color;
        Color legColor = leg.material.color;
        Color shirtsColor = shirts.material.color;
        Color shirts_sColor = shirts_s.material.color;
        Color shirts_bkColor = shirts_bk.material.color;
        Color skinColor = skin.material.color;
        Color tailColor = tail.material.color;
        Color tail_bottomColor = tail_bottom.material.color;
        Color uwagiColor = uwagi.material.color;
        Color uwagi_bkColor = uwagi_bk.material.color;

        button.material.color = Color.green;
        cheek.material.color = Color.green;
        hair_acc.material.color = Color.green;
        hair_front.material.color = Color.green;
        hair_side.material.color = Color.green;
        hairband.material.color = Color.green;
        leg.material.color = Color.green;
        shirts.material.color = Color.green;
        shirts_s.material.color = Color.green;
        shirts_bk.material.color = Color.green;
        skin.material.color = Color.green;
        tail.material.color = Color.green;
        tail_bottom.material.color = Color.green;
        uwagi.material.color = Color.green;
        uwagi_bk.material.color = Color.green;

        yield return new WaitForSeconds(0.1f);

        button.material.color = buttonColor;
        cheek.material.color = cheekColor;
        hair_acc.material.color = hair_accColor;
        hair_front.material.color = hair_frontColor;
        hair_side.material.color = hair_sideColor;
        hairband.material.color = hairbandColor;
        leg.material.color = legColor;
        shirts.material.color = shirtsColor;
        shirts_s.material.color = shirts_sColor;
        shirts_bk.material.color = shirts_bkColor;
        skin.material.color = skinColor;
        tail.material.color = tailColor;
        tail_bottom.material.color = tail_bottomColor;
        uwagi.material.color = uwagiColor;
        uwagi_bk.material.color = uwagi_bkColor;
    }

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
