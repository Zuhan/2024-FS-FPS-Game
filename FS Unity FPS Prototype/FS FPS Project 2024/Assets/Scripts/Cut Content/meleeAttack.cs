using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class meleeAttack : MonoBehaviour
{
    [SerializeField] Transform meleePos;
    [SerializeField] float meleeWait;
    public int damageAmount;
    public int damageDist;

    private Collider meleeCollider; // Reference to the collider

    void Start()
    {
        // Get the collider component
        meleeCollider = GetComponent<Collider>();
        // Disable the collider initially
        meleeCollider.enabled = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        // Enable the collider
        meleeCollider.enabled = true;

        // Wait for a short duration
        yield return new WaitForSeconds(0.1f);

        // Perform the raycast
        RaycastHit[] hits = Physics.RaycastAll(meleePos.position, meleePos.forward, damageDist);
        foreach (RaycastHit hit in hits)
        {
            // Check if the hit object is tagged as Enemy
            if (hit.transform.CompareTag("Enemy"))
            {
                // Check if the hit object has an IDamage interface
                IDamage dmg = hit.transform.GetComponent<IDamage>();
                if (dmg != null)
                {
                    // Apply damage to the enemy
                    dmg.TakeDamage(damageAmount);
                }
            }
        }

        // Disable the collider after the attack
        meleeCollider.enabled = false;
    }
}