using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] Rigidbody rb;

    [SerializeField] float damage;
    [SerializeField] float speed;
    [SerializeField] float destroyTime;
    bool hitHappend;
    float distanceToPlayery;
   

    void Start()
    {
        distanceToPlayery = gameManager.instance.player.transform.position.y + 1 - transform.position.y;
       

        rb.velocity = transform.forward * speed;


        if (distanceToPlayery != 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, distanceToPlayery, rb.velocity.z);
        }

        Destroy(gameObject, destroyTime);
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.isTrigger)
        {
            return;
        }

        IDamage dmg = other.GetComponent<IDamage>();
        

        if (other.CompareTag("Player") == true && dmg != null && !hitHappend)
        {
            dmg.TakeDamage(damage);
            hitHappend = true;
        }

        Destroy(gameObject);
    }

    public void AddDamage(float addedDamage)
    {
        damage += addedDamage;
    }

    public void RemoveDamage(float addedDamage)
    {
        damage -= addedDamage;
    }

}