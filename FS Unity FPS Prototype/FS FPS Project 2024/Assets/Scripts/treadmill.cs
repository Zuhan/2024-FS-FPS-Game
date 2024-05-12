using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treadmill : MonoBehaviour
{
    [SerializeField] float treadSpeed;
    private Vector3 move;
    //set to true for pos, false for neg
    [SerializeField] bool posOrNeg;
    //set to true for x, false for z
    [SerializeField] bool xOrZ;
    private float dir;
    // Start is called before the first frame update
    void Start()
    {
        if (posOrNeg)
        {
            dir = 1;
        }
        else
        {
            dir = -1;
        }
        if (xOrZ)
        {
            move = new Vector3(dir*treadSpeed,0.0f,0.0f);
        }
        else
        {
            move = new Vector3(0.0f, 0.0f, dir*treadSpeed);
        }
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
