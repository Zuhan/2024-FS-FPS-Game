using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponPickup : MonoBehaviour
{
    //THIS IS MY WEAPON, THERE IS ONLY ONE LIKE IT!!!!!!!!!!

    [SerializeField] weaponStats weapon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.getWeaponStats(weapon);
            Destroy(gameObject);
        }
    }
}