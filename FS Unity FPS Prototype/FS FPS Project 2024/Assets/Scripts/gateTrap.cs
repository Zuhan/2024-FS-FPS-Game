using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gateTrap : MonoBehaviour
{
    [SerializeField] GameObject gate;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gate.SetActive(true);
        }
    }
}