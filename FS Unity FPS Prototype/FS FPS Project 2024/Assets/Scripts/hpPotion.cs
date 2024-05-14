using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hpPotion : MonoBehaviour, IPickup
{
    [SerializeField] float hpToAdd;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void pickup()
    {
        gameManager.instance.playerScript.addHP(hpToAdd);
    }
}
