using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchDoor : MonoBehaviour, IfInteract
{
    [SerializeField] switches[] switches;
    [SerializeField] Renderer model;
    private bool allSwitched;
    private int switchCount;
    // Start is called before the first frame update
    void Start()
    {
        switchCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void interact()
    {
        checkSwitches();
        if (allSwitched)
        {
            StartCoroutine(openDoor());
        }
        else
        {

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
        model.material.color = Color.green;
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
