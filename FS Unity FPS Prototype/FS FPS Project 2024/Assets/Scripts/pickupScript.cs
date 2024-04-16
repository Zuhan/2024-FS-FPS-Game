using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupScript : MonoBehaviour, IPickup
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
        gameManager.instance.pointsChange(100);
        Destroy(gameObject);
    }
}
