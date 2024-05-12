using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupScript : MonoBehaviour, IPickup
{
    [SerializeField] int pointsToGain;
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
        gameManager.instance.pointsChange(pointsToGain);
        if (this.GetComponent<BillboardRenderer>())
        {
            gameManager.instance.playerScript.addHP(hpToAdd);
        }
        Destroy(gameObject);
    }
}
