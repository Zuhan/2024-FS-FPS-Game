using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class interactableObjectScript : MonoBehaviour, IfInteract
{
    [Header("---Audio---")]
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
    private Collider emptyGameObjectCollider;
    private bool isInteracting;
    private bool isFailing;
    // Start is called before the first frame update
    void Start()
    {
        emptyGameObjectCollider = emptyGameObject.GetComponent<Collider>();
    }
    public void interact()
    {
        Debug.Log("Interacting with the door");
        if (emptyGameObjectCollider != null)
        {
            emptyGameObjectCollider.enabled = true;
            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                Debug.Log("Loading Scene");
                emptyGameObject.GetComponent<sceneLoader>().LoadScene(sceneToLoad);
            }
            else
            {
                Debug.LogWarning("No scene to load specified.");
            }
        }

        int amount = gameManager.instance.points;
        if (amount < pointCost && !isFailing)
        {
            StartCoroutine(objInteractFail());
        }
        else if(amount >= pointCost && !isInteracting)
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
