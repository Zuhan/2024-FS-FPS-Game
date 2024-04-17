using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireMagic : MonoBehaviour, IDamage
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

        Instantiate(fire, collision.contacts[0].point, Quaternion.identity);

        Destroy(gameObject);

        transform.parent = null;

        hasHit = true;
    }
    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {

    }
}
