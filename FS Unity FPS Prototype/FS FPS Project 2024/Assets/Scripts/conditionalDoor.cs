using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class conditionalDoor : MonoBehaviour, IfInteract
{
    [Header("---Audio---")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip audOpen;
    [SerializeField] AudioClip audFail;
    [Range(0, 1)][SerializeField] float audOpenVol;
    [Range(0, 1)][SerializeField] float audFailVol;
    [Header("---Necessary Fields---")]
    [SerializeField] int objectsRequired;
    [SerializeField] Renderer model;
    [SerializeField] List<key> keys = new List<key>();
    //bool for determining what the door is for
    private bool isSingleScene;
    //int for counting how many keys are picked up for single scene usage
    private int count;
    private bool isInteracting;
    private bool isFailing;
    // Start is called before the first frame update
    void Start()
    {
        if (keys.Count != 0)
        {
            isSingleScene = true;
        }
    }
    //interact inherit from interface
    //handles both cases of multi scene usage and single scene usage
    public void interact()
    {
        if (isSingleScene)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                if (!keys[i].isActiveAndEnabled)
                {
                    count++;
                }
            }
            if(count < keys.Count && !isFailing)
            {
                StartCoroutine(openFail());
            }
            else if(count >= keys.Count && !isInteracting)
            {
                StartCoroutine(openDoor());
            }
            count = 0;
        }
        else
        {
            if (playerStats.keys.Count < objectsRequired && !isFailing && !isInteracting)
            {
                StartCoroutine(openFail());
            }
            else if(playerStats.keys.Count >= objectsRequired && !isInteracting)
            {
                StartCoroutine(openDoor());
                playerStats.keys.Clear();
            }
        }
    }
    //enumerator for opening the door
    IEnumerator openDoor()
    {
        isInteracting = true;
        aud.PlayOneShot(audOpen,audOpenVol);
        model.material.color = Color.green;
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
    //enumerator for not meeting conditions to open door
    IEnumerator openFail()
    {
        isFailing = true;
        aud.PlayOneShot(audFail, audFailVol);
        yield return new WaitForSeconds (1f);
        isFailing = false;
    }
}
