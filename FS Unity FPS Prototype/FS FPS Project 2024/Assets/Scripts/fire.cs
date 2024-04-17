using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire : MonoBehaviour, IDamage
{
    //AHAHAHAHAHAHA BURN BABY BURN

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
        StartCoroutine(DamageOverTime());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isAttached)
        {
            targetEnemy = collision.transform;
            initialOffset = transform.position - targetEnemy.position;
            isAttached = true;
        }
    }

    private void Update()
    {
        if (isAttached && targetEnemy != null)
        {
            transform.position = targetEnemy.position + initialOffset;
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

    public void TakeDamage(int damage)
    {
        
    }
}
