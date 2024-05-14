using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{

    [SerializeField] float damage;
    [SerializeField] float slowDownSpeed;
    [SerializeField] float slowCoolDown;
    bool hitHappend;
    bool slowDownApplied;

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
            if (slowDownApplied == false)
            {
                StartCoroutine(slowDown());
            }
        }
        hitHappend = false;
    }


    IEnumerator slowDown()
    {
        slowDownApplied = true;
        gameManager.instance.player.GetComponent<playerController>().AddedSpeed(-slowDownSpeed);
        yield return new WaitForSeconds(slowCoolDown);
        gameManager.instance.player.GetComponent<playerController>().AddedSpeed(slowDownSpeed);
        slowDownApplied = false;
    }

}