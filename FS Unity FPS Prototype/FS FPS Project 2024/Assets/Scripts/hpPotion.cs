using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hpPotion : MonoBehaviour, IPickup
{
    [SerializeField] float hpToAdd;
    public void pickup()
    {
        gameManager.instance.playerScript.addHP(hpToAdd);
    }
}
