using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class switchDoor : MonoBehaviour, IfInteract
{
    [Header("---Audio Fields---")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip audDoor;
    [Range(0, 1)][SerializeField] float audDoorVol;
    [SerializeField] AudioClip audFail;
    [Range(0, 1)][SerializeField] float audFailVol;
    [Header("---Fields for objects---")]
    [SerializeField] switches[] switches;
    [SerializeField] Renderer model;
    public TMP_Text numSwitched;
    public TMP_Text switchText;
    //private fields
    private bool allSwitched;
    private int switchCount;
    private int numSwitchedOn;
    private bool isInteracting;
    private bool isFailing;
    private bool rotating;
    // Start is called before the first frame update
    void Start()
    {
        switchCount = 0;
        numSwitched.text = switchCount.ToString();
        switchText.text = switches.Length.ToString();
    }
    public void interact()
    {
        checkSwitches();
        if (allSwitched && !isInteracting)
        {
            aud.PlayOneShot(audDoor, audDoorVol);
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            StartCoroutine(rotate(new Vector3(0,90,0),1));
        }
        else if(!allSwitched && !isFailing)
        {
            StartCoroutine(playFail());
        }
    }
    private void checkSwitches()
    {
        for (int i = 0; i < switches.Length; i++)
        {
            if (switches[i].isSwitchedOn())
            {
                switchCount++;
            }
        }
        if(switchCount == switches.Length)
        {
            allSwitched = true;
        }
        switchCount = 0;
    }
    public void UpdateSwitch()
    {
        numSwitchedOn++;
        numSwitched.text = numSwitchedOn.ToString();
    }
    IEnumerator playFail()
    {
        isFailing = true;
        aud.PlayOneShot(audFail,audFailVol);
        yield return new WaitForSeconds(1f);
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
}
