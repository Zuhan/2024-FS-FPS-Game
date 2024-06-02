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
            if (slowDownApplied == false && slowDownSpeed <= gameManager.instance.player.GetComponent<playerController>().CurrentSpeed())
            {
                StartCoroutine(slowDown());
            }
        }
        hitHappend = false;
    }


    IEnumerator slowDown()
    {
        slowDownApplied = true;
        gameManager.instance.player.GetComponent<playerController>().CooldownIncrement(1);
        gameManager.instance.player.GetComponent<playerController>().SprintingHit(false);
        gameManager.instance.player.GetComponent<playerController>().AddedSpeed(-slowDownSpeed);
        yield return new WaitForSeconds(slowCoolDown);
        gameManager.instance.player.GetComponent<playerController>().CooldownIncrement(-1); 
        gameManager.instance.player.GetComponent<playerController>().AddedSpeed(slowDownSpeed);
       
        if (gameManager.instance.player.GetComponent<playerController>().CooldownReturn() == 0)
        {
            gameManager.instance.player.GetComponent<playerController>().SprintingHit(true);
        }
        slowDownApplied = false;

    }

}