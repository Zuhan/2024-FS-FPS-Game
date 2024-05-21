using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oneWayDoor : MonoBehaviour
{
    [Header("---Fields for object and materials---")]
    [SerializeField] BoxCollider col;
    [SerializeField] Material material;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            col.enabled = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            col.enabled = true;
        }
    }
}
