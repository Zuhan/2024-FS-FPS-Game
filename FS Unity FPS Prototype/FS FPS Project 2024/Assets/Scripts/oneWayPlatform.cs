using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class oneWayPlatform : MonoBehaviour
{
    [Header("---Audio Field---")]
    [SerializeField] AudioSource aud;
    [Header("---Destination and Speed Fields---")]
    [SerializeField] GameObject endPoint;
    [SerializeField] float speed;
    bool isMoving;
    private Vector3 position;
    public void Start()
    {
        position = transform.position;
    }
    void FixedUpdate()
    {
        if (isMoving)
        {
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, endPoint.transform.position,step);
            if(Vector3.Distance(transform.position, endPoint.transform.position) < 0.001f)
            {
                isMoving = false;
                aud.Stop();
                aud.loop=false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform);
        if (other.CompareTag("Player"))
        {
            aud.Play();
            isMoving = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
        aud.Stop();
    }
    public void resetPosition()
    {
        transform.position = position;
        isMoving = false;
        aud.Stop();
    }
}
