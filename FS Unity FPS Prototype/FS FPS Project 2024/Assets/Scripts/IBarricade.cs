using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class IBarricade : MonoBehaviour , IDamage, IfInteract
{

    public GameObject[] planks;
    public BoxCollider boxCollider; 
    public int pointGain;
    public int HP;
    public int startingHP;
    public int modelNumber;
    bool barricadeBuilt;
    

    void Start()
    {
        // Initially, the model and box collider are disabled
        for (int i = 0; i < planks.Length; i++)
        {
            planks[i].SetActive(true);
        }

        boxCollider.enabled = true;
        HP = startingHP;
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
        //Debug.Log("Interact called.");
        StartCoroutine(objInteract());
        gameManager.instance.pointsChange(pointGain);
    }
    IEnumerator objInteract()
    {
        
        yield return new WaitForSeconds(1);
        BuildBarricade();
        gameManager.instance.hideInteractText();
        
    }
    public void TakeDamage(float damage)
    {
        HP -= (int)damage;     
        if (HP <= 0)
        {
            for (int i = 0; i < planks.Length; i++)
            {
                planks[i].SetActive(false);
            }
            boxCollider.enabled = false;
            barricadeBuilt = false;
            //Debug.Log("Barricade lost.");
        }
    }
    private void BuildBarricade()
    {
        for (int i = 0; i < planks.Length; i++)
        {
            planks[i].SetActive(true);
        }
        boxCollider.enabled = true;
        HP = startingHP;
        barricadeBuilt = true;
    }
}
