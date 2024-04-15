using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactableObjectScript : MonoBehaviour, IfInteract
{
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
        Destroy(gameObject);
    }
}
