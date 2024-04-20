using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionMagic : MonoBehaviour
{
    //I, DEREK, CALL UPON THE POWE TO VANQUISH MY ENEMIES, EXPLOSION!!!!!!!!!!!!!!!!

    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject explosion;

    [SerializeField] int damage;
    [SerializeField] float projectileSpeed;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;

    private bool hasHit = false;

    private Quaternion initialRotation;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.velocity = transform.forward * projectileSpeed;

        StartCoroutine(DestroyAfterTime());
    }
    public void SetInitialRotation(Quaternion rotation)
    {
        initialRotation = rotation;
        transform.rotation = initialRotation;
    }

    //Change Fire to trigger
    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit)
            return;

        GameObject hitObject = collision.gameObject;

        IDamage dmg = hitObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage);
        }

        Instantiate(explosion, collision.contacts[0].point, Quaternion.identity);

        transform.parent = null;

        hasHit = true;

        Destroy(gameObject);
    }
    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {

    }
}