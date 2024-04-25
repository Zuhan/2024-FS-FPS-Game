using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thunderMagic : MonoBehaviour, IDamage
{
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject thunder;

    [SerializeField] int baseDmg;
    [SerializeField] float flightSpeed;

    [SerializeField] int destroyAfterDuration;

    private bool hitEnemy;

    private Quaternion initialRotation;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * flightSpeed;

        StartCoroutine(DestroyAfterTime());
    }

    public void SetInitialRotation(Quaternion rot)
    {
        initialRotation = rot;
        transform.rotation = initialRotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hitEnemy)
        {
            return;
        }

        GameObject enemyToDamage = collision.gameObject;

        IDamage dmg = enemyToDamage.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(baseDmg);
        }

        Instantiate(thunder, collision.contacts[0].point, Quaternion.identity);
        transform.parent = null;
        hitEnemy = true;

        Destroy(gameObject);
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(destroyAfterDuration);
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {

    }
}
