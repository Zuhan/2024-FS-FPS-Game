using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire : MonoBehaviour
{
    [SerializeField] int damagePerTick;
    [SerializeField] float tickInterval;
    [SerializeField] float duration;

    [SerializeField] Rigidbody rb;

    private float timer = 0f;
    private Transform targetEnemy;
    private Vector3 initialOffset;

    private void Start()
    {
        Destroy(gameObject, duration);
        StartCoroutine(DamageOverTime());
    }

    private void Update()
    {
        if (targetEnemy != null)
        {
            transform.position = targetEnemy.position + initialOffset;
        }
    }

    public void SetTargetEnemy(Transform enemyTransform)
    {
        targetEnemy = enemyTransform;
        // Get the closest point on the enemy collider's surface to the fire's current position
        Collider enemyCollider = targetEnemy.GetComponent<Collider>();
        if (enemyCollider != null)
        {
            Vector3 closestPoint = enemyCollider.ClosestPoint(transform.position);
            Vector3 closestPointOffset = closestPoint - targetEnemy.position;
            transform.SetParent(targetEnemy);
            transform.localPosition = closestPointOffset;
        }
    }

    private IEnumerator DamageOverTime()
    {
        yield return new WaitForSeconds(0.5f);

        float timer = 0f;

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