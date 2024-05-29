using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioOnEntry : MonoBehaviour
{
    [SerializeField] private AudioClip sound;
    [Range(0, 1)][SerializeField] float volume;
    private int doOnce = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && doOnce == 0)
        {
            AudioSource.PlayClipAtPoint(sound, transform.position, volume);
            doOnce++;
        }
    }
}
