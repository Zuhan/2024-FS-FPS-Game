using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class expStaffBuy : MonoBehaviour, IfInteract
{
    [Header("---Fields for audio---")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip audBuy;
    [Range(0, 1)][SerializeField] float audBuyVol;
    [Header("---Fields for passing potion object and cost---")]
    [SerializeField] GameObject weapon;
    [SerializeField] int cost;
    [Header("---Fields for UI elements---")]
    [SerializeField] GameObject menuText;
    [SerializeField] GameObject menuText2;
    public TMP_Text costText;
    [SerializeField] GameObject failText;
    [SerializeField] GameObject soldOutText;

    private bool soldOut;
    // Start is called before the first frame update
    void Start()
    {
        costText.text = cost.ToString();
        if (playerStats.hasPurchasedExpStaff)
        {
            soldOut = true;
            displaySoldOut();
        }
    }
    public void interact()
    {
        if(gameManager.instance.points >= cost && !soldOut)
        {
            Instantiate(weapon,gameManager.instance.playerScript.transform.position, new Quaternion());
            gameManager.instance.pointsChange(-cost);
            playSound();
            playerStats.hasPurchasedExpStaff = true;
            soldOut = true;
            displaySoldOut();
        }
        else if (soldOut)
        {
            displaySoldOut();
        }
        else
        {
            StartCoroutine(displayFail());
        }
    }
    IEnumerator displayFail()
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
    private void displaySoldOut()
    {
        menuText.SetActive(false);
        menuText2.SetActive(false);
        soldOutText.SetActive(true);
    }
}
