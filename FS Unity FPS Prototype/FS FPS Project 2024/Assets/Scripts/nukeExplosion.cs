using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class nukeExplosion : MonoBehaviour, IDamage
{
    [SerializeField] float duration;
    [SerializeField] int damage;
    [SerializeField] float damageRadius;

    // Start is called before the first frame update
    void Start()
    {
        //Destroy gameObject after [SerializeField] duration
        Destroy(gameObject, duration);
    }

    // OnTrigger method to detect nearby objects and apply damage
    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object has IDamage interface
        IDamage damageable = other.GetComponent<IDamage>();
        if (damageable != null)
        {
            // Calculate distance between explosion center and the object
            float distance = Vector3.Distance(transform.position, other.transform.position);

            // Calculate falloff based on distance
            float falloff = 1f - Mathf.Clamp01(distance / damageRadius);

            // Calculate final damage
            int finalDamage = Mathf.RoundToInt(damage * falloff);

            // Apply damage to the object
            damageable.TakeDamage(finalDamage);
        }
    }

    public void TakeDamage(int damage)
    {

    }
}
