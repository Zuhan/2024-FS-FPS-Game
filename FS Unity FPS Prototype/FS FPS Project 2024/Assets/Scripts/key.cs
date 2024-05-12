using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour, IPickup
{
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
        playerStats.keys.Add(gameObject);

        //something something display UI menu saying you picked it up

        gameObject.SetActive(false);
    }
}
