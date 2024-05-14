using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gateTrap : MonoBehaviour
{
    [SerializeField] GameObject gate;
    //Create an OnTriggerEnter that enables the gate when the player enters the trigger
    //Create an OnTriggerExit that disables the gate when the player exits the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gate.SetActive(true);
        }
    }
}
