using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lavaScript : MonoBehaviour
{
    public int damage;
    private float time;
    private float diff;
    private float start;
    public float dmgDelay;
    // Start is called before the first frame update
    void Start()
    {
        time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        //establishing start time
        start = Time.time;
        if (other.gameObject.CompareTag("Player"))
        {
            IDamage dmg = other.GetComponent<IDamage>();
            if(dmg != null)
            {
                dmg.TakeDamage(damage);
            }
        }
    }
    //checking every frame for if player is in the lava
    private void OnTriggerStay(Collider other)
    {
        //checking current time
        time = Time.time;
        //checking if the difference is whatever delay that we decide to set or more so it can deal damage
        if(diff >= dmgDelay)
        {
            //only does dmg to player
            if (other.gameObject.CompareTag("Player"))
            {
                StartCoroutine(dealDamage(damage, other));
                //setting new times
                time = Time.time;
                start = Time.time;
            }
        }
        //setting the difference
        diff = time - start;
    }
    //resetting variables when player leaves lava
    private void OnTriggerExit(Collider other)
    {
        time = 0;
        diff = 0;
        start = 0;
    }
    //dealing damage to player
    IEnumerator dealDamage(int amount, Collider other)
    {
        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage);
        }
        yield return new WaitForSeconds(1f);
        
    }
}
