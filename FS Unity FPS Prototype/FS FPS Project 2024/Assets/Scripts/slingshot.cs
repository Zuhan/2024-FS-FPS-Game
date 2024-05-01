using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slingshot : MonoBehaviour
{
    [SerializeField] Transform shootPOS;
    [SerializeField] GameObject bullet;
    [SerializeField] AudioClip snapSound;

    private AudioSource audioSource; 

    void Start()
    {
        enabled = false;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateBullet();
        }
    }

    void CreateBullet()
    {
        Debug.Log("Bullet Created");
        Instantiate(bullet, shootPOS.position, shootPOS.rotation);

        // Play the shoot sound
        if (audioSource != null && snapSound != null)
        {
            audioSource.PlayOneShot(snapSound);
        }
    }

    public void EnableSlingshot()
    {
        Debug.Log("Slingshot Enabled");
        enabled = true;
    }
    public void DisableSlingshot()
    {
        Debug.Log("Slingshot Disabled");
        enabled = false;
    }
}