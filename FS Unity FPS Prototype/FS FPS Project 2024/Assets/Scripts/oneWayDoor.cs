using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oneWayDoor : MonoBehaviour
{
    [SerializeField] BoxCollider col;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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
