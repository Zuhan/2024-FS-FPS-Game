using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class interactableObjectScript : MonoBehaviour, IfInteract
{
    [SerializeField] Renderer model;
    [SerializeField] int pointCost;
    [SerializeField] int HP;
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
        int amount = gameManager.instance.points;
        if (amount < pointCost)
        {
            StartCoroutine(objInteractFail());
        }
        else
        {
            StartCoroutine(objInteract());
            gameManager.instance.pointsChange(-pointCost);
        }
    }
    public int getCost()
    {
        return pointCost;
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
        gameManager.instance.showInteractText(pointCost);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameManager.instance.showInteractText(pointCost);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameManager.instance.hideInteractText();
        }
    }
}
