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

    private float dirMod;
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
    //call direction for with negative num to swap directions, if no direction change wanted leave positive or zero
    public void changeDirection(char axis, float direction)
    {
        if(direction < 0)
        {
            dirMod = -1;
        }
        else
        {
            dirMod = 1;
        }
        if (axis.Equals('x') || axis.Equals('X'))
        {
            move.z = 0;
            move.x = dirMod * treadSpeed;
        }
        else
        {
            move.x = 0;
            move.z = dirMod * treadSpeed;
        }
    }
}
