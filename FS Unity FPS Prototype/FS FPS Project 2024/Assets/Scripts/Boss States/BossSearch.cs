using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class BossSearch : MonoBehaviour, IDamage
{
    [SerializeField] private string stateName;
    [SerializeField] private IBossState currentState;
    [SerializeField] public SphereCollider trigger;
    [SerializeField] public Animator anim;
    //added by bug healthbar
    [SerializeField] Image healthbar;
    //added by bug hurt audio source
    [SerializeField] AudioSource HurtAudioSource;

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

    [SerializeField] public GameObject healAura;
    [SerializeField] public CapsuleCollider hitbox;

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
    //audio for hurt noise added by bug
    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audHurt;
    [Range(0, 1)][SerializeField] float audVolHurt;

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
    private bool HurtOnCooldown;

    void OnEnable()
    {
        HP = maxHP;
        UpdateEnemyUI();
        currentState = idleState;
        trigger.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
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
        //added by bug call udpate enemy UI
        UpdateEnemyUI();
        //audio source added by bug
        if(HurtOnCooldown == false)
        {
            StartCoroutine(PlayHurtAudio());
        }


        if (HP <= 0)
        {
            Destroy(gameObject);

            gameManager.instance.win();
        }
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

    //Play hurt audio added by bug
    IEnumerator PlayHurtAudio()
    {
        HurtOnCooldown = true;
        HurtAudioSource.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audVolHurt);
        yield return new WaitForSeconds(audHurt.Length);
        HurtOnCooldown = false;
    }

    //added by bug update enemy UI
    void UpdateEnemyUI()
    {
        healthbar.fillAmount = HP / maxHP;
    }


}
