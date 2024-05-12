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
    [SerializeField] Material mat;

    private Color color;
    // Start is called before the first frame update
    void Start()
    {
        isActive = true;
        color.a = mat.color.a;
        color.r = mat.color.r;
        color.g = mat.color.g;
        color.b = mat.color.b;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
        {
            isActive = true;
            StartCoroutine(reappear2(timeToReappear));
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(disappear2(timeToDisappear));
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
        mat.color = color;
    }
    IEnumerator disappear2(float dur)
    {
        for (float t = 0f; t < dur; t += Time.deltaTime)
        {
            Color c = mat.color;
            c.a = c.a - 0.001f;
            mat.color = c;
            yield return null;
        }
        render.enabled = false;
        objectCollider.enabled = false;
        trigger.enabled = false;
        isActive = false;
    }
    IEnumerator reappear2(float dur)
    {
        render.enabled = true;
        for (float t = 0f; t < dur; t += Time.deltaTime)
        {
            Color c = mat.color;
            c.a = c.a + 0.001f;
            mat.color = c;
            yield return null;
        }
        
        objectCollider.enabled = true;
        trigger.enabled = true;
        mat.color = color;
    }
}
