using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireMagic : MonoBehaviour, IDamage
{
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject fire;

    [SerializeField] int damage;
    [SerializeField] float fireballSpeed;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;

    private bool hasHit = false;

    private Quaternion initialRotation;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Rigidbody component
        Rigidbody rb = GetComponent<Rigidbody>();

        // Set the initial velocity of the fireball
        rb.velocity = transform.forward * fireballSpeed;

        // Start coroutine to destroy the fireball after a certain time
        StartCoroutine(DestroyAfterTime());
    }

    // Method to set the initial rotation of the fireball
    public void SetInitialRotation(Quaternion rotation)
    {
        initialRotation = rotation;
        // Set the initial rotation immediately when the fireball is instantiated
        transform.rotation = initialRotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the projectile has already hit something
        if (hasHit)
            return;

        // Get the object hit by the projectile
        GameObject hitObject = collision.gameObject;

        // Apply initial damage
        IDamage dmg = hitObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage);
        }

        // Instantiate fire object at collision point
        Instantiate(fire, collision.contacts[0].point, Quaternion.identity);

        // Destroy the fireball
        Destroy(gameObject);

        // Unparent the fireball from the object it hits
        transform.parent = null;

        hasHit = true;
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
