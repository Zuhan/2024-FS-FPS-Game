using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class potionBuy : MonoBehaviour, IfInteract
{
    [Header("---Fields for audio---")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip audBuy;
    [Range(0, 1)][SerializeField] float audBuyVol;
    [Header("---Fields for passing potion object and cost---")]
    [SerializeField] hpPotion potion;
    [SerializeField] int cost;
    [Header("---Fields for UI elements---")]
    [SerializeField] GameObject menuText;
    [SerializeField] GameObject menuText2;
    public TMP_Text costText;
    [SerializeField] GameObject failText;

    
    // Start is called before the first frame update
    void Start()
    {
        costText.text = cost.ToString();
    }
    public void interact()
    {
        if(gameManager.instance.points >= cost)
        {
            playerStats.potions.Add(potion);
            gameManager.instance.pointsChange(-cost);
            playSound();
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
