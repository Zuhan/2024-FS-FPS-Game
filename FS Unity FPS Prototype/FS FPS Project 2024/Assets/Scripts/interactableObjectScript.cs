using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class interactableObjectScript : MonoBehaviour, IfInteract
{
    [SerializeField] Renderer model;
    [SerializeField] int pointCost;

    [SerializeField] GameObject emptyGameObject;
    [SerializeField] string sceneToLoad;
    private Collider emptyGameObjectCollider;

    // Start is called before the first frame update
    void Start()
    {
        emptyGameObjectCollider = emptyGameObject.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void interact()
    {
        Debug.Log("Interacting with the door");
        if (emptyGameObjectCollider != null)
        {
            emptyGameObjectCollider.enabled = true;
            // Load the scene using the SceneLoader attached to the emptyGameObject
            emptyGameObject.GetComponent<sceneLoader>().LoadScene(sceneToLoad);
        }

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
