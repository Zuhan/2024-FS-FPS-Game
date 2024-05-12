using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thunderMagic : MonoBehaviour, IDamage
{
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject thunder;

    public int currJumps;
    [SerializeField] int maxJumps;

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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (hitEnemy)
            {
                return;
            }

            GameObject enemyHit = collision.gameObject;
            GameObject thunderBall = Instantiate(thunder, collision.contacts[0].point, Quaternion.identity);
            thunderBall.transform.SetParent(collision.gameObject.transform);

            hitEnemy = true;
            IDamage dmg = enemyHit.GetComponent<IDamage>();
            //Debug.Log(dmg);
            if (dmg != null)
            {
                dmg.TakeDamage(baseDmg);
                //Debug.Log("Dealt " + baseDmg + " to " + enemyHit.name);
            }
        }

        Destroy(gameObject);

    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(destroyAfterDuration);
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {

    }
}
