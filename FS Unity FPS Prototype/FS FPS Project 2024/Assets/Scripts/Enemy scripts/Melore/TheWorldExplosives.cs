using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWorldExplosives : MonoBehaviour
{
    [SerializeField] SphereCollider trigger;

    // Start is called before the first frame update
    void OnEnable()
    {
        trigger = GetComponent<SphereCollider>();
        trigger.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
