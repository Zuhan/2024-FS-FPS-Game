using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire : MonoBehaviour
{
    //AHAHAHAHAHAHA BURN BABY BURN

    [SerializeField] int damagePerTick;
    [SerializeField] float tickInterval;
    [SerializeField] float duration;

    [SerializeField] Rigidbody rb;

    private float timer = 0f;
    private Transform targetEnemy;
    private Vector3 initialOffset;

    private void Start()
    {
        //Destroy gameObject after [SerializeField] duration
        Destroy(gameObject, duration);
        StartCoroutine(DamageOverTime());
    }

    private void Update()
    {
        // Check if the fire is attached to an enemy
        if (targetEnemy != null)
        {
            // Update the fire's position relative to the enemy's center of mass
            transform.position = targetEnemy.position + initialOffset;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && targetEnemy == null)
        {
            // Set the target enemy
            targetEnemy = other.transform;
            // Calculate the initial offset from the enemy's center of mass
            Rigidbody enemyRigidbody = targetEnemy.GetComponent<Rigidbody>();
            if (enemyRigidbody != null)
            {
                Vector3 centerOfMassOffset = enemyRigidbody.centerOfMass;
                initialOffset = transform.position - (targetEnemy.position + centerOfMassOffset);
                // Attach the fire to the center of mass of the enemy
                transform.SetParent(targetEnemy);
            }
            // Disable the fire's collider to prevent further OnTriggerEnter calls
            Collider fireCollider = GetComponent<Collider>();
            if (fireCollider != null)
            {
                fireCollider.enabled = false;
            }
        }
    }

    private IEnumerator DamageOverTime()
    {
        while (timer < duration)
        {
            ApplyDamageOverTime();

            yield return new WaitForSeconds(tickInterval);
            timer += tickInterval;
        }
        transform.SetParent(null);
        Destroy(gameObject);
    }

    private void ApplyDamageOverTime()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, transform.localScale.x / 2f);
        foreach (Collider collider in colliders)
        {
            IDamage dmg = collider.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.TakeDamage(damagePerTick);
            }
        }
    }
}