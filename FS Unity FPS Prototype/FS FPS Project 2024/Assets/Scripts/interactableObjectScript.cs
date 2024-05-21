using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class interactableObjectScript : MonoBehaviour, IfInteract
{
    [Header("---Audio Fields---")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip audOpen;
    [SerializeField] AudioClip audFail;
    [Range(0, 1)][SerializeField] float audOpenVol;
    [Range(0, 1)][SerializeField] float audFailVol;
    [Header("---Fields for changing color and point cost---")]
    [SerializeField] Renderer model;
    [SerializeField] int pointCost;
    [Header("---Fields for scene change---")]
    [SerializeField] GameObject emptyGameObject;
    [SerializeField] string sceneToLoad;
    //private bools for preventing multiple interactions
    private Collider emptyGameObjectCollider;
    private bool isInteracting;
    private bool isFailing;
    private bool isInTrigger;
    // Start is called before the first frame update
    void Start()
    {
        emptyGameObjectCollider = emptyGameObject.GetComponent<Collider>();
    }
    public void interact()
    {
        int amount = gameManager.instance.points;
        if (amount < pointCost && !isFailing)
        {
            StartCoroutine(objInteractFail());
        }
        else if(amount >= pointCost && !isInteracting)
        {
            Debug.Log("Interacting with the door");
            if (emptyGameObjectCollider != null)
            {
                emptyGameObjectCollider.enabled = true;
                if (!string.IsNullOrEmpty(sceneToLoad))
                {
                    Debug.Log("Loading Scene");
                    gameManager.instance.pointsChange(-pointCost);
                    StartCoroutine(objInteract());
                    emptyGameObject.GetComponent<sceneLoader>().LoadScene(sceneToLoad);
                }
                else
                {
                    Debug.LogWarning("No scene to load specified.");
                }
            }
            else
            {
                StartCoroutine(objInteract());
                gameManager.instance.pointsChange(-pointCost);
            }
        }
    }
    public int getCost()
    {
        return pointCost;
    }
    IEnumerator objInteract()
    {
        isInteracting = true;
        model.material.color = Color.green;
        aud.PlayOneShot(audOpen,audOpenVol);
        yield return new WaitForSeconds(1);
        gameManager.instance.hideInteractText();
        isInteracting=false;
        Destroy(gameObject);
    }
    IEnumerator objInteractFail()
    {
        isFailing=true;
        gameManager.instance.hideInteractText();
        gameManager.instance.showInteractFail();
        aud.PlayOneShot(audFail,audFailVol);
        yield return new WaitForSeconds(1);
        gameManager.instance.hideInteractFail();
        gameManager.instance.showInteractText(pointCost);
        isFailing = false;
        if (!isInTrigger)
        {
            gameManager.instance.hideInteractText();
            gameManager.instance.hideInteractFail();
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInTrigger=true;
            gameManager.instance.showInteractText(pointCost);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInTrigger=false;
            gameManager.instance.hideInteractText();
        }
    }
}
