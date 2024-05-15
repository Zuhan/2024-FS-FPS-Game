using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MagicBullet : MonoBehaviour
{

    [SerializeField] Rigidbody rb;

    [SerializeField] float DMG;
    [SerializeField] float flightSpeed;
    [SerializeField] float destroyAfterDuration;
    [SerializeField] float forceMultiplier;

    bool hasHit;
    float distanceToPlayerY;
    float distanceToPlayerX;


    void Start()
    {
        Vector3 playerDir = (gameManager.instance.player.transform.position - transform.position).normalized;

        rb.velocity = playerDir * flightSpeed;

        if (distanceToPlayerY != 0 && distanceToPlayerX != 0)
        {
            rb.velocity = new Vector3(rb.velocity.x * forceMultiplier, distanceToPlayerY, rb.velocity.z);
            rb.velocity = new Vector3(distanceToPlayerX * forceMultiplier, rb.velocity.y, rb.velocity.z);
        }

        Destroy(gameObject, destroyAfterDuration);
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.isTrigger)
        {
            return;
        }

        IDamage dmg = other.GetComponent<IDamage>();


        if (other.CompareTag("Player") == true && dmg != null && !hasHit)
        {
            dmg.TakeDamage(DMG);
            hasHit = true;
        }

        Destroy(gameObject);
    }

    public void AddDamage(float addedDamage)
    {
        DMG += addedDamage;
    }

    public void RemoveDamage(float addedDamage)
    {
        DMG -= addedDamage;
    }

}
