using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire : MonoBehaviour, IDamage
{
    [SerializeField] int damagePerTick;
    [SerializeField] float tickInterval;
    [SerializeField] float duration;

    [SerializeField] Rigidbody rb;

    private float timer = 0f;
    private bool isAttached = false;
    private Transform targetEnemy;
    private Vector3 initialOffset;

    private void Start()
    {
        // Start applying damage over time
        StartCoroutine(DamageOverTime());
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Log to check if collision is detected
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        // Check if the collision is with an enemy and fire is not already attached
        if (collision.gameObject.CompareTag("Enemy") && !isAttached)
        {
            // Attach the fire to the enemy
            targetEnemy = collision.transform;
            // Calculate initial offset
            initialOffset = transform.position - targetEnemy.position;
            // Update flag
            isAttached = true;

            Debug.Log("Fire magic hit enemy capsule.");
        }
        else
        {
            Debug.Log("Fire magic hit: " + collision.gameObject.name);
        }
    }

    private void Update()
    {
        // If the fire is attached to an enemy, follow its position
        if (isAttached && targetEnemy != null)
        {
            // Update fire position relative to the enemy and initial offset
            transform.position = targetEnemy.position + initialOffset;
        }
    }

    private IEnumerator DamageOverTime()
    {
        while (timer < duration)
        {
            // Apply damage over time
            ApplyDamageOverTime();

            yield return new WaitForSeconds(tickInterval);
            timer += tickInterval;
        }

        // Detach the fire from the enemy and destroy it
        transform.SetParent(null);
        Destroy(gameObject);
    }

    private void ApplyDamageOverTime()
    {
        // Find all colliders within the radius of the fire
        Collider[] colliders = Physics.OverlapSphere(transform.position, transform.localScale.x / 2f);
        foreach (Collider collider in colliders)
        {
            // Check if collider belongs to an object that implements IDamage
            IDamage dmg = collider.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.TakeDamage(damagePerTick);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        // Nothing here for fire
    }
}
