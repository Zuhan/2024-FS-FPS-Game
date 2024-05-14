using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeTrap : MonoBehaviour
{
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip audSpike;
    [Range(0, 1)][SerializeField] float audSpikeVol;
    [SerializeField] AudioClip audSpikeDown;
    [Range(0, 1)][SerializeField] float audSpikeDownVol;
    [SerializeField] float delay;
    [SerializeField] GameObject spikes;
    private Vector3 move;
    private Vector3 moveDown;
    bool raised;
    bool inSpikes;
    // Start is called before the first frame update
    void Start()
    {
        move = new Vector3(0, 0.85f, 0);
        moveDown = new Vector3(0, -0.85f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(raised && !inSpikes)
        {
            raised = false;
            StartCoroutine(lowerSpikes());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            if (!raised)
            {
                StartCoroutine(raiseSpikes());
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inSpikes = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        inSpikes = false;
    }
    IEnumerator raiseSpikes()
    {
        yield return new WaitForSeconds(delay);
        spikes.transform.Translate(move);
        aud.PlayOneShot(audSpike,audSpikeVol);
        raised = true;
    }
    IEnumerator lowerSpikes()
    {
        yield return new WaitForSeconds(delay);
        spikes.transform.Translate(moveDown);
        aud.PlayOneShot(audSpikeDown, audSpikeDownVol);
        raised = false;
    }
}
