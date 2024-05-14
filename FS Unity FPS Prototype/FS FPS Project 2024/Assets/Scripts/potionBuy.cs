using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class potionBuy : MonoBehaviour, IfInteract
{
    [SerializeField] hpPotion potion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void interact()
    {
        Debug.Log("Potion added");
        Debug.Log(playerStats.potions);
        playerStats.potions.Add(potion);
    }
}
