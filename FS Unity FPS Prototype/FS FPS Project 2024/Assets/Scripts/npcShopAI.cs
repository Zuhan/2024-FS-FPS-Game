using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class npcShopAI : MonoBehaviour
{
    [SerializeField] Renderer model;
    [SerializeField] Animator anim;
    [SerializeField] Transform headPos;

    [SerializeField] AudioSource aud;

    [SerializeField] int animSpeedTrans;

    [SerializeField] AudioClip[] voiceOver;
    [Range(0, 1)][SerializeField] float audVoiceVol;

    bool playerInRange;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            aud.PlayOneShot(voiceOver[Random.Range(0, voiceOver.Length)], audVoiceVol);
        }
    }
}