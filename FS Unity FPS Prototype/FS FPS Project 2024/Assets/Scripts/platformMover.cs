using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformMover : MonoBehaviour

    //read pathScript.cs please
{
    [Header("---Path Field(Needs a pathScript object)---")]
    [SerializeField] pathScript path;
    [Header("---Fields for speed and stop duration---")]
    [SerializeField] float speed;
    [SerializeField] float stopTime;
    //Private fields for determining the target point
    private int targetIndex;
    private Transform prev;
    private Transform target;
    private float timeToPoint;
    private float elapsed;
    private bool isStopped;
    void Start()
    {
        targetNext();
    }
    void FixedUpdate()
    {
        elapsed += Time.deltaTime;
        float completed = elapsed / timeToPoint;
        completed = Mathf.SmoothStep(0,1,completed);
        transform.position = Vector3.Lerp(prev.position, target.position, completed);
        transform.rotation = Quaternion.Lerp(prev.rotation, target.rotation, completed);
        if (completed >= 1 && !isStopped)
        {
            StartCoroutine(stopAtNextPoint());
        }
    }
    private void targetNext()
    {
        prev = path.getPoint(targetIndex);
        targetIndex = path.getNext(targetIndex);
        target = path.getPoint(targetIndex);
        elapsed = 0;
        float distance = Vector3.Distance(prev.position, target.position);
        timeToPoint = distance/speed;
    }
    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform);
    }
    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
    IEnumerator stopAtNextPoint()
    {
        isStopped = true;
        yield return new WaitForSeconds(stopTime);
        targetNext();
        isStopped = false;
    }
}
