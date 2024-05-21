using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

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
    public TMP_Text numKeys;
    public TMP_Text keyText;
    //bool for determining what the door is for
    private bool isSingleScene;
    //int for counting how many keys are picked up for single scene usage
    private int count;
    //bools for preventing interactions from happening multiple times in quick succession
    private bool isInteracting;
    private bool isFailing;
    //int for how many keys the player currently has
    private int curKeys = 0;
    //bool for if the door is currently opening
    bool rotating;
    // Start is called before the first frame update
    void Start()
    {
        if (keys.Count != 0)
        {
            isSingleScene = true;
            keyText.text = keys.Count.ToString();
            numKeys.text = curKeys.ToString();
        }
        else
        {
            keyText.text = objectsRequired.ToString();
            numKeys.text = playerStats.keys.Count.ToString();
        }
    }
    public void Update()
    {
        if(keys.Count == 0)
        {
            numKeys.text = playerStats.keys.Count.ToString();
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
                aud.PlayOneShot(audOpen, audOpenVol);
                StartCoroutine(rotate(new Vector3(0,90,0),1));
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
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
                aud.PlayOneShot(audOpen, audOpenVol);
                StartCoroutine(rotate(new Vector3(0, 90, 0), 1));
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
                playerStats.keys.Clear();
            }
        }
    }
    //enumerator for not meeting conditions to open door
    IEnumerator openFail()
    {
        isFailing = true;
        aud.PlayOneShot(audFail, audFailVol);
        yield return new WaitForSeconds (1f);
        isFailing = false;
    }
    IEnumerator rotate(Vector3 rot, float dur)
    {
        if (rotating)
        {
            yield break;
        }
        isInteracting = true;
        rotating = true;
        Vector3 newRot = transform.eulerAngles + rot;
        Vector3 curRot = transform.eulerAngles;
        float count = 0;
        while (count < dur)
        {
            count += Time.deltaTime;
            transform.eulerAngles = Vector3.Lerp(curRot, newRot, count);
            yield return null;
        }
        rotating = false;
    }
    public void updateKey()
    {
        curKeys++;
        numKeys.text = curKeys.ToString();
    }
}
