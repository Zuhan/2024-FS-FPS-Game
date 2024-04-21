using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireMagic : MonoBehaviour
{
    //I, DEREK, CALL UPON THE POWER OF FIRE. HHHHHHAAAAAAA

    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject fire;

    [SerializeField] int damage;
    [SerializeField] float fireballSpeed;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;

    private bool hasHit = false;

    private Quaternion initialRotation;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.velocity = transform.forward * fireballSpeed;

        StartCoroutine(DestroyAfterTime());
    }
    public void SetInitialRotation(Quaternion rotation)
    {
        initialRotation = rotation;
        transform.rotation = initialRotation;
    }

    //Change Fire to trigger
    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit)
            return;

        GameObject hitObject = collision.gameObject;

        IDamage dmg = hitObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage);
        }

        GameObject newFire = Instantiate(fire, collision.contacts[0].point, Quaternion.identity);

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (enemyRigidbody != null)
            {
                Vector3 centerOfMassOffset = enemyRigidbody.centerOfMass;
                newFire.transform.SetParent(collision.gameObject.transform);
                newFire.transform.localPosition = centerOfMassOffset;
            }
        }

        // Create a fixed joint to attach the fire object to the collided object
        FixedJoint joint = newFire.AddComponent<FixedJoint>();
        joint.connectedBody = collision.rigidbody;

        hasHit = true;

        Destroy(gameObject);
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}