using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class geyser : MonoBehaviour
{
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip audGeyser;
    [Range(0, 1)][SerializeField] float audGeyserVol;
    [SerializeField] float speed;
    [SerializeField] ParticleSystem particles;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.addVelocityY(speed);
            aud.PlayOneShot(audGeyser,audGeyserVol);
            particles.Play();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.addVelocityY(speed);
        }
    }
}
