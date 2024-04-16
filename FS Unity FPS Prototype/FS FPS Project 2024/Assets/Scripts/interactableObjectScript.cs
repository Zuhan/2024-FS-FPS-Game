using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class interactableObjectScript : MonoBehaviour, IfInteract
{
    [SerializeField] Renderer model;
    [SerializeField] int pointCost;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void interact(int amount)
    {
        if (amount < pointCost)
        {
            StartCoroutine(objInteractFail());
        }
        else
        {
            StartCoroutine(objInteract());
            gameManager.instance.pointsChange(-amount);
        }
    }
    IEnumerator objInteract()
    {
        model.material.color = Color.green;
        yield return new WaitForSeconds(1);
        gameManager.instance.hideInteractText();
        Destroy(gameObject);
    }
    IEnumerator objInteractFail()
    {
        gameManager.instance.hideInteractText();
        gameManager.instance.showInteractFail();
        yield return new WaitForSeconds(1);
        gameManager.instance.hideInteractFail();
        gameManager.instance.showInteractText();
    }
}
