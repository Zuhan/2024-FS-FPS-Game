using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour, IPickup
{
    [Header("---Audio---")]
    /*[SerializeField] AudioSource aud;
    [SerializeField] AudioClip audPickup;
    [Range(0, 1)][SerializeField] float audPickupVol;*/
    [Header("---Bool for determining if key is for one scene or multiple scenes---")]
    [SerializeField] bool isSingleScene;

    //pickup inherit from interface
    //handles single and multiple scene use cases
    public void pickup()
    {
        if (isSingleScene)
        {
            gameObject.SetActive(false);
        }
        else
        {
            playerStats.keys.Add(gameObject);
            gameObject.SetActive(false);
        }
    }
}
