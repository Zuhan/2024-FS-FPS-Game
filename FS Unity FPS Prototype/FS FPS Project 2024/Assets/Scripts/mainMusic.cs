using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainMusic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Play the music
            GetComponent<AudioSource>().Play();
        }
        //Turn off collider to prevent further OnTriggerEnter calls
        GetComponent<Collider>().enabled = false;
    }
}
