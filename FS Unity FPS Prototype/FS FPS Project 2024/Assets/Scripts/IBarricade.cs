using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IBarricade : MonoBehaviour , IDamage, IfInteract
{

    public Renderer model;
    public BoxCollider boxCollider; 
    public int pointGain;
    public int HP;
    public int startingHP;
    bool barricadeBuilt;

    void Start()
    {
        // Initially, the model and box collider are disabled
        model.enabled = false;
        boxCollider.enabled = false;
        startingHP = HP;
    }
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            gameManager.instance.showBarricadeText(pointGain);
            boxCollider.enabled = true;
        }
       
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            gameManager.instance.hideBarricadeText();
            if (!barricadeBuilt)
            {
                boxCollider.enabled = false;
            }

        }
    }


    public void interact()
    {
        Debug.Log("Interact called.");
        StartCoroutine(objInteract());
        gameManager.instance.pointsChange(pointGain);
    }
    IEnumerator objInteract()
    {
        
        model.material.color = Color.green;
        yield return new WaitForSeconds(1);
        BuildBarricade();
        gameManager.instance.hideInteractText();
        
    }
    public void TakeDamage(int damage)
    {
        HP -= damage;     
        if (HP <= 0)
        {
            boxCollider.enabled = false;
            model.enabled = false;
            barricadeBuilt = false;
            Debug.Log("Barricade lost.");
        }
    }
    private void BuildBarricade()
    {
        model.enabled = true;
        boxCollider.enabled = true;
        HP = startingHP;
        barricadeBuilt = true;
    }
}
