using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupScript : MonoBehaviour, IPickup
{
    [Header("---Fields to play audio of pickups---")]
    [SerializeField] AudioClip audPoints;
    [Range(0, 1)][SerializeField] float audPointsVol;
    [SerializeField] AudioClip audHP;
    [Range(0, 1)][SerializeField] float audHPVol;
    [Header("---Fields to change values of pickups---")]
    [SerializeField] int pointsToGain;
    [SerializeField] float hpToAdd;
    public void pickup()
    {
        if (this.GetComponent<BillboardRenderer>())
        {
            playHP();
            gameManager.instance.playerScript.addHP(hpToAdd);
        }
        else
        {
            playPoint();
            gameManager.instance.pointsChange(pointsToGain);
        }
        Destroy(gameObject);
    }
    private void playPoint()
    {
        AudioSource.PlayClipAtPoint(audPoints,transform.position,audPointsVol);
    }
    private void playHP()
    {
        AudioSource.PlayClipAtPoint(audHP, transform.position, audHPVol);
    }
}
