using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mortarStart : MonoBehaviour
{
    [SerializeField] mortarShell mortar;
    [SerializeField] float damage;

    private bool damageTaken;
    private bool fired;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mortar.hasLandedFunc())
        {
            StartCoroutine(stopMortar());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&!fired)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            mortar.startMortar();
            fired = true;
        }
    }
    IEnumerator dealDamage(Collider other)
    {
        Debug.Log("xdd");
        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null)
        {
            Debug.Log("xdd");
            dmg.TakeDamage(damage);
            gameManager.instance.playerScript.addVelocityY(10);
        }
        yield return null;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (mortar.hasLandedFunc() && !damageTaken)
            {
                Debug.Log("xdd");
                StartCoroutine(dealDamage(other));
                damageTaken = true;
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(false);
                transform.gameObject.SetActive(false);
            }
        }
    }
    IEnumerator stopMortar()
    {
        yield return new WaitForSeconds(.5f);
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.gameObject.SetActive(false);
    }
}
