using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEditorInternal;
using UnityEngine;

public class thunder : MonoBehaviour
{
    [SerializeField] int baseDmg;
    [SerializeField] int castRate;
    [SerializeField] int chainRange;

    public int currJumps;
    [SerializeField] int maxJumps;
    [SerializeField] float chainJumpDelay;

    [SerializeField] float delay;

    [SerializeField] Rigidbody rb;
    [SerializeField] bool hasHit;

    public GameObject chainLightningPrefab;
    public GameObject objectHit;

    private Transform targetEnemy;
    private Vector3 initialOffset;

    BoxCollider chainTrigger;


    // Start is called before the first frame update
    void Start()
    {
        chainTrigger = GetComponent<BoxCollider>();

        Destroy(gameObject, delay);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit)
            return;

        objectHit = collision.gameObject;

        if (objectHit.CompareTag("Enemy"))
        {
            Debug.Log("Hit " + collision.gameObject.name);

            initialOffset = transform.position - objectHit.transform.position;

            //activate trigger for chain
            StartCoroutine(WaitForChain(chainJumpDelay, objectHit.GetComponent<Collider>()));

            Destroy(gameObject);
        }
        //objectHit.gameObject.tag = "Enemy";
    }

    
    void chainNearbyEnemy(Vector3 center, float range)
    {
        Collider[] hits = Physics.OverlapSphere(center, range);
        Debug.Log("Entered chainEnemy function");

        foreach(var hit in hits)
        {
            if (hit.gameObject.CompareTag("Enemy") && currJumps < maxJumps)
            {
                Debug.Log("Enemy in range: " + hit.name);
                if (!hasHit)
                {
                    IDamage dmg = hit.GetComponent<IDamage>();
                    if (dmg != null)
                    {
                        Debug.Log("Dealt damage to " + hit.gameObject.name);
                        hasHit = true;
                        currJumps++;
                        dmg.TakeDamage(baseDmg);
                        hasHit = false;
                    }
                } 
            }
        }
    }

//private void OnCollisionExit(Collision collision)
//{
//    GameObject objectHit = collision.gameObject;

//    objectHit.tag = "Enemy";
//    currentHits--;
//}

//private void OnTriggerEnter(Collider other)
//{
//    GameObject currHitEnemy = other.gameObject;

//    while (currJumps < maxJumps)
//    {
//        if (currHitEnemy.CompareTag("Enemy"))
//        {
//            Debug.Log("Attempting jump to: " + other.name);

//            currJumps++;
//            targetEnemy = other.transform;
//            GameObject thunderToChain = Instantiate(chainLightningPrefab, targetEnemy.transform.position, Quaternion.identity);

//            if (targetEnemy != null)
//            {
//                //Vector3 com = targetEnemy.position;
//                //initialOffset = transform.position = (targetEnemy.position + com).normalized;
//                ApplyChainDamage(other);
//                //transform.SetParent(targetEnemy);
//            }
//            //targetEnemy.gameObject.tag = "EnemyHitByThunder";
//            //ApplyChainDamage(other);
//        }
//        break;
//    }
//    Debug.Log("curr > max");
//}

    private void ApplyChainDamage(Collider other)
    {        
        IDamage dmg = other.GetComponent<IDamage>();
        if(dmg != null)
        {
            Debug.Log("Attempting to Deal Damage to: " + other.name);
            dmg.TakeDamage(baseDmg);
        }
    }

    IEnumerator WaitForChain(float delay, Collider other)
    {
        Debug.Log("Jumping to... " + other.gameObject.name);
        chainNearbyEnemy(objectHit.transform.position, chainRange);
        yield return new WaitForSeconds(delay);
        
    }
    void FixedUpdate()
    {
       
    }
}
