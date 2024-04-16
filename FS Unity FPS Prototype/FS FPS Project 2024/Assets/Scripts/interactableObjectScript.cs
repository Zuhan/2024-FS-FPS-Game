using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class interactableObjectScript : MonoBehaviour, IfInteract
{
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
        StartCoroutine(objInteract());
    }
    IEnumerator objInteract()
    {
        model.material.color = Color.green;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
