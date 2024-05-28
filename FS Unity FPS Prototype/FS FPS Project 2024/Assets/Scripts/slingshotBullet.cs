using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slingshotBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;
    bool hitHappend;

    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            return;
        }

        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null && !hitHappend)
        {
            dmg.TakeDamage(damage);
            hitHappend = true;
        }
    }
}