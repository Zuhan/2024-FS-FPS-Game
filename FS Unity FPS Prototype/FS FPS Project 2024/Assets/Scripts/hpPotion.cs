using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hpPotion : MonoBehaviour, IPickup
{
    [Header("---Audio Fields---")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip audClip;
    [Range(0,1)][SerializeField] float audVol;
    [Header("---HP Amount Field---")]
    [SerializeField] float hpToAdd;
    public void pickup()
    {
        gameManager.instance.playerScript.addHP(hpToAdd);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerStats.potions.Add(this);
            AudioSource.PlayClipAtPoint(audClip,transform.position,audVol);
            gameObject.SetActive(false);
        }
    }
}
