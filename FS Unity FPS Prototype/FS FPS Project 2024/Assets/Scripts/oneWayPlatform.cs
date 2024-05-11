using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class oneWayPlatform : MonoBehaviour
{
    [SerializeField] GameObject endPoint;
    [SerializeField] float speed;
    bool isMoving;
    void FixedUpdate()
    {
        if (isMoving)
        {
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, endPoint.transform.position,step);
            if(Vector3.Distance(transform.position, endPoint.transform.position) < 0.001f)
            {
                isMoving = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform);
        if (other.CompareTag("Player"))
        {
            isMoving = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
}
