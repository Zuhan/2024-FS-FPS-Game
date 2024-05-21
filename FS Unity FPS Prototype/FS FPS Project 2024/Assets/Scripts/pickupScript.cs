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
        if (pointsToGain > 0)
        {
            playPoint();
            gameManager.instance.pointsChange(pointsToGain);
        }
        if (hpToAdd > 0)
        {
            playHP();
            gameManager.instance.playerScript.addHP(hpToAdd);
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
