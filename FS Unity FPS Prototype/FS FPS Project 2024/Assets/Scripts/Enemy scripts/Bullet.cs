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
    [SerializeField] float homingSpeed;
    [SerializeField] float HomingStrength;
    [SerializeField] float DistanceTillHoming;

    bool HomingActive;
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


    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, gameManager.instance.player.transform.position);

        if (distanceToPlayer <=  DistanceTillHoming && HomingActive == false)
        {
            StartCoroutine(Homing());
        }


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


    IEnumerator Homing()
    {
        HomingActive = true;
        float distanceToPlayerx = gameManager.instance.player.transform.position.x - transform.position.x;
        float distanceToPlayery = gameManager.instance.player.transform.position.y + 1 - transform.position.y;
        float distanceToPlayerz = gameManager.instance.player.transform.position.z - transform.position.z;
        rb.velocity = new Vector3(rb.velocity.x + (distanceToPlayerx * HomingStrength), rb.velocity.y + (distanceToPlayery * HomingStrength), rb.velocity.z + (distanceToPlayerz * HomingStrength));
        yield return new WaitForSeconds(homingSpeed);
        HomingActive = false;
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