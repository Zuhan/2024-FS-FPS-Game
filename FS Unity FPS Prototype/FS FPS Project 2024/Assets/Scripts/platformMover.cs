using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformMover : MonoBehaviour
{
    [SerializeField] pathScript path;
    [SerializeField] float speed;
    private int targetIndex;
    private Transform prev;
    private Transform target;
    private float timeToPoint;
    private float elapsed;
    // Start is called before the first frame update
    void Start()
    {
        targetNext();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        elapsed += Time.deltaTime;
        float completed = elapsed / timeToPoint;
        completed = Mathf.SmoothStep(0,1,completed);
        transform.position = Vector3.Lerp(prev.position, target.position, completed);
        transform.rotation = Quaternion.Lerp(prev.rotation, target.rotation, completed);
        if (completed >= 1)
        {
            targetNext();
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
}
