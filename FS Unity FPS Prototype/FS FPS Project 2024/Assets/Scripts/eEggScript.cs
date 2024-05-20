using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eEggScript : MonoBehaviour
{
    [SerializeField] AudioSource aud;

    [SerializeField] AudioClip[] voiceLine;
    [Range(0, 1)][SerializeField] float audVoiceVol;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            aud.PlayOneShot(voiceLine[Random.Range(0, voiceLine.Length)], audVoiceVol);
        }
    }
}
