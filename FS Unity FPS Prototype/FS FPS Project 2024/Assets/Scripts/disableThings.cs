using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableThings : MonoBehaviour
{
    [SerializeField] GameObject thing;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            thing.SetActive(false);
        }
    }
}