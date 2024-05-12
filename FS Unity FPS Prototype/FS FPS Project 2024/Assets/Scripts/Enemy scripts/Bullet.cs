using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] Rigidbody rb;

    [SerializeField] float damage;
    [SerializeField] float speed;
    [SerializeField] float destroyTime;
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

        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null && !hitHappend)
        {
            dmg.TakeDamage(damage);
            hitHappend = true;
        }

        Destroy(gameObject);
    }

}