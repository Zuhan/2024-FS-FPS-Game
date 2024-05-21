using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mortarStart : MonoBehaviour
{
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip audFire;
    [Range(0, 1)][SerializeField] float audFireVol;
    [SerializeField] AudioClip audLand;
    [Range(0, 1)][SerializeField] float audLandVol;
    [SerializeField] mortarShell mortar;
    [SerializeField] float damage;
    [SerializeField] ParticleSystem particles;
    Collider col;
    private bool damageTaken;
    private bool fired;
    private bool hasPlayed;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mortar.hasLandedFunc() && !hasPlayed)
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
            aud.PlayOneShot(audFire,audFireVol);
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
                //aud.PlayOneShot(audLand, audLandVol);
                /*transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(false);
                col.gameObject.SetActive(false);*/
                //transform.gameObject.SetActive(false);
            }
        }
    }
    IEnumerator stopMortar()
    {
        yield return new WaitForSeconds(.1f);
        
        
        if (!hasPlayed)
        {
            aud.PlayOneShot(audLand, audLandVol);
            particles.Play();
        }
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        col.enabled = false;
        hasPlayed = true;
        //transform.gameObject.SetActive(false);
    }
}
