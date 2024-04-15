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

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);

        StartCoroutine(DamageOverTime());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamage>() != null)
        {
            IDamage dmg = other.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.TakeDamage(damage);
            }
            Destroy(gameObject);
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
}
