using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conditionalDoor : MonoBehaviour, IfInteract
{
    [SerializeField] int objectsRequired;
    [SerializeField] Renderer model;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void interact()
    {
        if(playerStats.keys.Count < objectsRequired)
        {

        }
        else
        {
            StartCoroutine(openDoor());
            playerStats.keys.Clear();
        }
    }
    IEnumerator openDoor()
    {
        model.material.color = Color.green;
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
    IEnumerator openFail()
    {
        yield return new WaitForSeconds (1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        
    }
}
