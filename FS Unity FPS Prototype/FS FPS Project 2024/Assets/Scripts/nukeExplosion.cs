using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class nukeExplosion : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] int damage;
    [SerializeField] float damageRadius;
    [SerializeField] float maxDamageDistance;

    SphereCollider explosionCollider;
    float currentRadius;
    [SerializeField] float expandSpeed;
    [SerializeField] float targetRadius;

    // Start is called before the first frame update
    void Start()
    {
        explosionCollider = GetComponent<SphereCollider>();
        currentRadius = 1f;
        Destroy(gameObject, duration);
        StartCoroutine(ExpandCollider());
    }

    IEnumerator ExpandCollider()
    {
        while (currentRadius < targetRadius)
        {
            currentRadius += expandSpeed * Time.deltaTime;
            explosionCollider.radius = currentRadius;
            yield return null;
        }
    }

    // OnTrigger method to detect nearby objects and apply damage
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collider Stayed: " + other.name);
        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null)
        {
            float distance = Vector3.Distance(transform.position, other.transform.position);

            float damageMultiplier = Mathf.Clamp01(1f - (distance / maxDamageDistance));

            int calculatedDamage = Mathf.RoundToInt(damage * damageMultiplier);

            dmg.TakeDamage(damage);
        }
    }

    // Draw the damage radius gizmo for visualization
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}