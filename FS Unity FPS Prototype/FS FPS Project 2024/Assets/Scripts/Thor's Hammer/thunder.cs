using System.Collections;
using System.Collections.Generic;
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


    // Start is called before the first frame update
    void Start()
    {
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

            StartCoroutine(WaitForChain(chainJumpDelay));

            Destroy(gameObject);
        }
    }

    
    void ChainNearbyEnemy(Vector3 center, float range)
    {
        Collider[] hits = Physics.OverlapSphere(center, range);
        //Debug.Log("Entered chainEnemy function");

        foreach(var hit in hits)
        {
            if (hit.gameObject.CompareTag("Enemy") && currJumps < maxJumps)
            {
                //Debug.Log("Enemy in range: " + hit.name);
                if (!hasHit)
                {
                    IDamage dmg = hit.GetComponent<IDamage>();
                    if (dmg != null)
                    {
                        Instantiate(chainLightningPrefab, hit.bounds.center, Quaternion.identity);
                        //Debug.Log("Dealt damage to " + hit.gameObject.name);
                        hasHit = true;
                        currJumps++;
                        dmg.TakeDamage(baseDmg);
                        hasHit = false;
                    }
                } 
            }
        }
    }

    IEnumerator WaitForChain(float delay)
    {
        ChainNearbyEnemy(objectHit.transform.position, chainRange);
        yield return new WaitForSeconds(delay);
    }
}
