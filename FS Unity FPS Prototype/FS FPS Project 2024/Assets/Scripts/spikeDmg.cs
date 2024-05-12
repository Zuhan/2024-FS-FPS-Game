using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeDmg : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float dmgDelay;

    private float time;
    private float diff;
    private float start;
    // Start is called before the first frame update
    void Start()
    {
        time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        start = Time.time;
        if (other.gameObject.CompareTag("Player"))
        {
            IDamage dmg = other.GetComponent<IDamage>();
            if(dmg!= null)
            {
                dmg.TakeDamage(damage);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            time = Time.time;
            if (diff >= dmgDelay)
            {
                if (other.CompareTag("Player"))
                {
                    StartCoroutine(dealDamage(other));
                    time = Time.time;
                    start = Time.time;
                }
            }
            diff = time - start;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            time = 0f;
            diff = 0f;
            start = 0f;
        }
    }
    IEnumerator dealDamage(Collider other)
    {
        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage);
        }
        yield return new WaitForSeconds(dmgDelay);

    }
}
