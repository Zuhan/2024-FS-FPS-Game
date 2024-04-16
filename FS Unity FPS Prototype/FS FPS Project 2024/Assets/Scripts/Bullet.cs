using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] Rigidbody rb;

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;

    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null)
        {
            dmg.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

}
