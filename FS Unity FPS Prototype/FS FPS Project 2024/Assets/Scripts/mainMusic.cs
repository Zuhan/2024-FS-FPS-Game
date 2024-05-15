using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainMusic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<AudioSource>().Play();
        }
        GetComponent<Collider>().enabled = false;
    }
}