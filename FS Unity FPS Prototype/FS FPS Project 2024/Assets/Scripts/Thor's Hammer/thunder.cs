using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEditorInternal;
using UnityEngine;

public class thunder : MonoBehaviour
{
    [SerializeField] int baseDmg;
    [SerializeField] int castRate;
    [SerializeField] int castRange;

    [SerializeField] int chainRange;
    [SerializeField] int maxChainJumps;
    public int currentHits;
    [SerializeField] float chainJumpDelay;

    [SerializeField] float delay;

    [SerializeField] Rigidbody rb;

    public GameObject chainLightningPrefab;

    private bool hasHit = false;
    private Transform targetEnemy;
    private Vector3 initialOffset;

    BoxCollider chainTrigger;


    // Start is called before the first frame update
    void Start()
    {
        chainTrigger = GetComponent<BoxCollider>();
        currentHits = 0;

        Destroy(gameObject, delay);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit)
            return;
        
        GameObject objectHit = collision.gameObject;

        if (objectHit.CompareTag("Enemy"))
        {
            targetEnemy = collision.transform;
            initialOffset = transform.position - targetEnemy.position;

            //activate trigger for chain
            chainTrigger.enabled = true;

            
            //objectHit.gameObject.tag = "EnemyHitByThunder";
        }

        chainTrigger.enabled = false;
        hasHit = true;

        Destroy(gameObject);
        //objectHit.gameObject.tag = "Enemy";
    }

    //private void OnCollisionExit(Collision collision)
    //{
    //    GameObject objectHit = collision.gameObject;

    //    objectHit.tag = "Enemy";
    //    currentHits--;
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (!hasHit && other.CompareTag("Enemy"))
        {
            targetEnemy = other.transform;

            Debug.Log("The God of Thunder says...");            

            while (currentHits < maxChainJumps)
            {
                Instantiate(chainLightningPrefab, targetEnemy.transform.position, Quaternion.identity);
                
                if(targetEnemy != null)
                {
                    Debug.Log("rbCheck reached");
                    Vector3 com = targetEnemy.position;
                    initialOffset = transform.position = (targetEnemy.position + com).normalized;

                    transform.SetParent(targetEnemy);
                }

                //targetEnemy.gameObject.tag = "EnemyHitByThunder";
                currentHits++;
            }

            ApplyChainDamage(other);
            chainTrigger.enabled = false;
        }
    }

    private void ApplyChainDamage(Collider other)
    {        
        IDamage dmg = other.GetComponent<IDamage>();
        if(dmg != null)
        {
            Debug.Log("Attempting to Deal Damage to: " + other.name);
            dmg.TakeDamage(baseDmg);
        }
    }
    void Update()
    {
       
    }
}
