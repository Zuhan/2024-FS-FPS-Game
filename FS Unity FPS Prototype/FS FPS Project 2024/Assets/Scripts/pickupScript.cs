using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupScript : MonoBehaviour, IPickup
{
    [SerializeField] int pointsToGain;

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
        //Debug.Log("Item picked up.");
        gameManager.instance.winByPoints();
        //Game Manager points?
        gameManager.instance.pointsChange(pointsToGain);
        //PointsManager Points?
        PointsManager.Instance.AddPoints(pointsToGain);
        Destroy(gameObject);
    }
}
