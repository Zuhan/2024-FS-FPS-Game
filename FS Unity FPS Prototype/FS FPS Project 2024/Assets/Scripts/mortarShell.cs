using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mortarShell : MonoBehaviour
{
    [SerializeField] GameObject endPoint;
    [SerializeField] float speed;
    private bool isMoving;
    private bool hasLanded;
    // Start is called before the first frame update
    void Start()
    {
        //isMoving = false;
        hasLanded = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            var step = speed* Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, endPoint.transform.position,step);
            if(Vector3.Distance(transform.position,endPoint.transform.position) < 0.001f)
            {
                isMoving = false;
                hasLanded = true;
            }
        }
    }
    public void startMortar()
    {
        isMoving = true;
    }
    public bool hasLandedFunc()
    {
        return hasLanded;
    }
}
