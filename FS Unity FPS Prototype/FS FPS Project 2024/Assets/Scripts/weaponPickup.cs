using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponPickup : MonoBehaviour
{
    //THIS IS MY WEAPON, THERE IS ONLY ONE LIKE IT!!!!!!!!!!

    [SerializeField] weaponStats weapon;
    [SerializeField] int weaponNumber;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.getWeaponStats(weapon);
            Destroy(gameObject);
            gameManager.instance.ShowWeaponIcon(weaponNumber);
        }
    }
}