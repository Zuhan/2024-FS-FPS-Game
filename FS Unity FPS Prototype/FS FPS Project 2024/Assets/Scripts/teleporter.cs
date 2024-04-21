using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleporter : MonoBehaviour
{
    public GameObject destination;
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
        if (other.gameObject.CompareTag("Player"))
        {
            gameManager.instance.playerScript.controller.enabled = false;
            if (destination.CompareTag("Teleporter"))
            {
                gameManager.instance.player.transform.position = destination.transform.GetChild(0).GetChild(1).transform.position;
            }
            else
            {
                gameManager.instance.player.transform.position = destination.transform.position;

            }
            gameManager.instance.playerScript.controller.enabled = true;
        }
    }
}
