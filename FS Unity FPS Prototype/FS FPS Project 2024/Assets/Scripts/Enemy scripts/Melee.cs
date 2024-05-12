using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{

    [SerializeField] float damage;
    bool hitHappend;


    private void OnTriggerEnter(Collider other)
    {

        if (other.isTrigger)
        {
            return;
        }

        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null && !hitHappend)
        {
            dmg.TakeDamage(damage);
            hitHappend = true;
        }
        hitHappend = false;
    }

}