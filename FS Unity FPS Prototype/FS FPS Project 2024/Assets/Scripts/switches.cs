using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class switches : MonoBehaviour, IfInteract
{
    [Header("---Audio Fields---")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip audSwitch;
    [Range(0, 1)][SerializeField] float audSwitchVol;
    [Header("---Game Objects---")]
    [SerializeField] GameObject menu;
    [SerializeField] switchDoor switchDoor;
    //private fields
    private bool switchedOn;
    private bool rotating;
    void Start()
    {
        switchedOn = false;
    }
    public void interact()
    {
        if (!switchedOn)
        {
            StartCoroutine(switchOn());
            StartCoroutine(rotate(new Vector3(-90,0,0),1));
            switchedOn = true;
        }
    }
    public bool isSwitchedOn()
    {
        return switchedOn;
    }
    IEnumerator switchOn()
    {
        aud.PlayOneShot(audSwitch,audSwitchVol);
        menu.SetActive(false);
        switchDoor.UpdateSwitch();
        yield return new WaitForSeconds(0f);
    }
    IEnumerator rotate(Vector3 rot,float dur)
    {
        if (rotating)
        {
            yield break;
        }
        rotating = true;
        Vector3 newRot = transform.eulerAngles + rot;
        Vector3 curRot = transform.eulerAngles;
        float count = 0;
        while(count < dur)
        {
            count += Time.deltaTime;
            transform.eulerAngles = Vector3.Lerp(curRot,newRot,count);
            yield return null;
        }
        rotating = false;
    }
}
