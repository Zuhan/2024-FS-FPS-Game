using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class potionBuy : MonoBehaviour, IfInteract
{
    [SerializeField] AudioSource aud;

    [SerializeField] hpPotion potion;
    [SerializeField] int cost;
    [SerializeField] GameObject menuText;
    [SerializeField] GameObject menuText2;
    public TMP_Text costText;
    [SerializeField] GameObject failText;

    [SerializeField] AudioClip audBuy;
    [Range(0, 1)][SerializeField] float audBuyVol;
    // Start is called before the first frame update
    void Start()
    {
        costText.text = cost.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void interact()
    {
        if(gameManager.instance.points >= cost)
        {
            playerStats.potions.Add(potion);
            gameManager.instance.pointsChange(-cost);
            playSound();
            //play audio or something
        }
        else
        {
            StartCoroutine(display());
        }
    }
    IEnumerator display()
    {
        menuText.SetActive(false);
        menuText2.SetActive(false);
        failText.SetActive(true);
        yield return new WaitForSeconds(1f);
        failText.SetActive(false);
        menuText.SetActive(true);
        menuText2.SetActive(true);
    }
    private void playSound()
    {
        aud.PlayOneShot(audBuy, audBuyVol);
    }
}
