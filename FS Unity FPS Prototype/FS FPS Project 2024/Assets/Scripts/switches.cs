using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class switches : MonoBehaviour, IfInteract
{
    [SerializeField] GameObject menu;
    private bool switchedOn;
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
        menu.SetActive(false);
        yield return new WaitForSeconds(1f);
    }
}
