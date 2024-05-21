using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleporter : MonoBehaviour
{
    [Header("---Audio Fields---")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip audTp;
    [Range(0, 1)][SerializeField] float audTpVol;
    [Header("---Destination Field---")]
    public GameObject destination;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameManager.instance.playerScript.controller.enabled = false;
            if (destination.CompareTag("Teleporter"))
            {
                gameManager.instance.player.transform.position = destination.transform.GetChild(0).GetChild(1).transform.position;
                aud.PlayOneShot(audTp, audTpVol);
            }
            else
            {
                gameManager.instance.player.transform.position = destination.transform.position;
                aud.PlayOneShot(audTp, audTpVol);
            }
            gameManager.instance.playerScript.controller.enabled = true;
        }
    }
}
