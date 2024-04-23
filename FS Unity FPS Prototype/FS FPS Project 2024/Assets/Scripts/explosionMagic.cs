using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionMagic : MonoBehaviour
{
    //Explosion Magic Being Shelved


    //I, DEREK, CALL UPON THE POWE TO VANQUISH MY ENEMIES, EXPLOSION!!!!!!!!!!!!!!!!

    //[SerializeField] Rigidbody rb;
    //[SerializeField] GameObject explosion;

    //[SerializeField] int damage;
    //[SerializeField] int damageRadius;
    //[SerializeField] float projectileSpeed;
    //[SerializeField] int speed;
    //[SerializeField] int destroyTime;

    //private bool hasHit = false;

    //private Quaternion initialRotation;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    Rigidbody rb = GetComponent<Rigidbody>();

    //    rb.velocity = transform.forward * projectileSpeed;

    //    StartCoroutine(DestroyAfterTime());
    //}
    //public void SetInitialRotation(Quaternion rotation)
    //{
    //    initialRotation = rotation;
    //    transform.rotation = initialRotation;
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (hasHit)
    //        return;

    //    GameObject hitObject = collision.gameObject;

    //    IDamage dmg = hitObject.GetComponent<IDamage>();
    //    if (dmg != null)
    //    {
    //        float distance = Vector3.Distance(transform.position, collision.collider.transform.position);
    //        float falloff = 1f - Mathf.Clamp01(distance / damageRadius);
    //        int finalDamage = Mathf.RoundToInt(damage * falloff);

    //        dmg.TakeDamage(damage);
    //    }

    //    Instantiate(explosion, collision.contacts[0].point, Quaternion.identity);

    //    transform.parent = null;

    //    hasHit = true;

    //    Destroy(gameObject);
    //}
    //private IEnumerator DestroyAfterTime()
    //{
    //    yield return new WaitForSeconds(destroyTime);
    //    Destroy(gameObject);
    //}
}