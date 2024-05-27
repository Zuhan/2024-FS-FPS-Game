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
    [SerializeField] float chainJumpDelay = 0.5f;

    [SerializeField] float delay = 1f;

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

            StartCoroutine(WaitForChain());

            Destroy(gameObject);
        }
    }

    
    void ChainNearbyEnemy(Vector3 center, float range)
    {
        Collider[] hits = Physics.OverlapSphere(center, range);

        foreach(var hit in hits)
        {
            if (hit.gameObject.CompareTag("Enemy") && currJumps < maxJumps)
            {
                if (!hasHit)
                {
                    Instantiate(chainLightningPrefab, hit.bounds.center, Quaternion.identity);
                    IDamage dmg = hit.GetComponent<IDamage>();
                    if (dmg != null)
                    {
                        hasHit = true;
                        currJumps++;
                        dmg.TakeDamage(baseDmg);
                        hasHit = false;
                    }
                } 
            }
        }
    }

    IEnumerator WaitForChain()
    {
        ChainNearbyEnemy(objectHit.transform.position, chainRange);
        yield return new WaitForSeconds(chainJumpDelay);
    }
}
