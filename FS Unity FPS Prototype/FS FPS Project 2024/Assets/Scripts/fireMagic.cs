using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireMagic : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int damage;
    [SerializeField] int DOT;
    [SerializeField] float DOTDuration;
    [SerializeField] float DOTRadius;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;

    private bool hasHit = false;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        StartCoroutine(DestroyAfterTime());
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the projectile has already hit something
        if (hasHit)
            return;

        // Get the object hit by the projectile
        GameObject hitObject = collision.gameObject;

        // Check if the object is on a layer that should interact with the fireball
        if (hitObject.layer != LayerMask.NameToLayer("IgnoreProjectile"))
        {
            // If the object is not the player, stop the projectile and apply damage
            if (!hitObject.CompareTag("Player"))
            {
                Rigidbody rb = GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;

                // Stick the projectile to the object it hits
                transform.parent = hitObject.transform;

                // Align the rotation of the projectile with the surface normal of the object it hits
                transform.rotation = Quaternion.FromToRotation(Vector3.forward, collision.contacts[0].normal);

                hasHit = true;

                // Apply damage over time
                StartCoroutine(DamageOverTime());
            }
        }
    }
    IEnumerator DamageOverTime()
    {
        float timer = 0f;
        while (timer < DOTDuration)
        {
            yield return new WaitForSeconds(DOT);
            ApplyDamageOverTime();
            timer += DOT;
        }
    }

    void ApplyDamageOverTime()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, DOTRadius);
        foreach (Collider collider in colliders)
        {
            // Check if collider belongs to an object that implements IDamage
            IDamage dmg = collider.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.TakeDamage(damage);
            }
        }
    }
    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
