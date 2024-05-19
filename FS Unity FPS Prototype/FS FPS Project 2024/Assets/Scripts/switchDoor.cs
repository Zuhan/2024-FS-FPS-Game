using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class switchDoor : MonoBehaviour, IfInteract
{
    [SerializeField] AudioSource aud;
    [SerializeField] switches[] switches;
    [SerializeField] Renderer model;
    private bool allSwitched;
    private int switchCount;
    public TMP_Text numSwitched;
    public TMP_Text switchText;
    private int numSwitchedOn;
    [SerializeField] AudioClip audDoor;
    [Range(0, 1)][SerializeField] float audDoorVol;
    [SerializeField] AudioClip audFail;
    [Range(0, 1)][SerializeField] float audFailVol;
    private bool isInteracting;
    private bool isFailing;
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
            StartCoroutine(openDoor());
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
    IEnumerator openDoor()
    {
        isInteracting = true;
        model.material.color = Color.green;
        aud.PlayOneShot(audDoor,audDoorVol);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
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
}
