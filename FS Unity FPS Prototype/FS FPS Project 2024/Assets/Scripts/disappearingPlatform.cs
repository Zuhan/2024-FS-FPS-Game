using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disappearingPlatform : MonoBehaviour
{
    [Header("-----Time Settings-----")]
    [SerializeField] float timeToDisappear;
    [SerializeField] float timeToReappear;

    [Header("-----Things that need to be disabled and reenabled-----")]
    [SerializeField] MeshRenderer render;
    [SerializeField] BoxCollider objectCollider;
    [SerializeField] BoxCollider trigger;
    private bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
        {
            isActive = true;
            StartCoroutine(reappear());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(disappear());
        }
    }
    IEnumerator disappear()
    {
        Debug.Log("disappear called");
        yield return new WaitForSeconds(timeToDisappear);
        render.enabled = false;
        objectCollider.enabled = false;
        trigger.enabled = false;
        isActive = false;
    }
    IEnumerator reappear()
    {
        Debug.Log("reappear called");
        yield return new WaitForSeconds(timeToReappear);
        render.enabled = true;
        objectCollider.enabled = true;
        trigger.enabled = true;
    }

}
