using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treadmill : MonoBehaviour
{
    [SerializeField] float treadSpeed;
    private Vector3 move;
    // Start is called before the first frame update
    void Start()
    {
        move = new Vector3 (-treadSpeed, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.controller.Move(move * Time.deltaTime);
        }
    }
}
