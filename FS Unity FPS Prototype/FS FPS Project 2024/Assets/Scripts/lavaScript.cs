using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lavaScript : MonoBehaviour
{
    public int damage;
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
    public void OnTriggerEnter(Collider other)
    {
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
    private void OnTriggerStay(Collider other)
    {
        time = Time.time;
        if(diff >= 1)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                StartCoroutine(dealDamage(damage, other));
                time = Time.time;
                start = Time.time;
            }
        }
        diff = time - start;
    }
    private void OnTriggerExit(Collider other)
    {
        time = 0;
        diff = 0;
        start = 0;
    }
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
