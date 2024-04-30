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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy") && targetEnemy == null)
        {
            targetEnemy = collision.collider.transform;
            Rigidbody enemyRigidbody = targetEnemy.GetComponent<Rigidbody>();
            if (enemyRigidbody != null)
            {
                Vector3 centerOfMassOffset = enemyRigidbody.centerOfMass;
                initialOffset = transform.position - (targetEnemy.position + centerOfMassOffset);
                transform.SetParent(targetEnemy);
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