using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oneWayDoorExit : MonoBehaviour
{
    [SerializeField] oneWayDoor door;
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
            door.changeMaterial(1);
        }
    }
}
