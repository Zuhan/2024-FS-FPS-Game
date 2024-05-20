using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class switches : MonoBehaviour, IfInteract
{
    [SerializeField] AudioSource aud;

    [SerializeField] GameObject menu;
    [SerializeField] switchDoor switchDoor;
    private bool switchedOn;
    [SerializeField] AudioClip audSwitch;
    [Range(0, 1)][SerializeField] float audSwitchVol;
    bool rotating;
    // Start is called before the first frame update
    void Start()
    {
        switchedOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        
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
    public void OnTriggerEnter(Collider other)
    {
        
    }
    public void OnTriggerExit(Collider other)
    {
        
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
