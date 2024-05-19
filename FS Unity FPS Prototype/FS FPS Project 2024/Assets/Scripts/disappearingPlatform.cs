using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class disappearingPlatform : MonoBehaviour
{
    [Header("---Audio---")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip audClip;
    [Range(0f, 1f)][SerializeField] float audVol;

    [Header("---Time Settings---")]
    [SerializeField] float timeToDisappear;
    [SerializeField] float timeToReappear;

    [Header("---Things that need to be disabled and reenabled---")]
    [SerializeField] MeshRenderer render;
    [SerializeField] BoxCollider objectCollider;
    [SerializeField] BoxCollider trigger;
    private bool isActive;
    [SerializeField] Material mat;

    private Color color;
    private bool isPlaying;
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
            StartCoroutine(reappear(timeToReappear));
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(disappear(timeToDisappear));
        }
    }
    IEnumerator disappear(float dur)
    {
        for (float t = 0f; t < dur; t += Time.deltaTime)
        {
            Color c = mat.color;
            c.a = c.a - 0.001f;
            mat.color = c;
            yield return null;
            if(!isPlaying && dur - t <= 1)
            {
                UnityEngine.Debug.Log("xdd");
                StartCoroutine(playSound());
            }
        }
        render.enabled = false;
        objectCollider.enabled = false;
        trigger.enabled = false;
        isActive = false;
    }
    IEnumerator reappear(float dur)
    {
        render.enabled = true;
        for (float t = 0f; t < dur; t += Time.deltaTime)
        {
            Color c = mat.color;
            c.a = c.a + 0.001f;
            mat.color = c;
            yield return null;
            if (!isPlaying && dur - t <= 1)
            {
                UnityEngine.Debug.Log("ddx");
                StartCoroutine(playSound());
            }
        }
        objectCollider.enabled = true;
        trigger.enabled = true;
        mat.color = color;
    }
    IEnumerator playSound()
    {
        isPlaying = true;
        aud.PlayOneShot(audClip,audVol);
        yield return new WaitForSeconds(1.2f);
        isPlaying = false;
    }
}
