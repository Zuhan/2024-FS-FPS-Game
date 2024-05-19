using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slingshot : MonoBehaviour
{
    [SerializeField] Transform shootPOS;
    [SerializeField] GameObject bullet;
    [SerializeField] AudioClip snapSound;

    [SerializeField] private float fireCooldown = 0.5f;
    private float lastFireTime;

    private AudioSource audioSource; 

    void Start()
    {
        enabled = false;
        audioSource = GetComponent<AudioSource>();
        DisableAudio();
    }

    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            if (Input.GetMouseButtonDown(0) && Time.time - lastFireTime > fireCooldown)
            {
                CreateBullet();
                lastFireTime = Time.time;
                StartCoroutine(displayCooldown());
            }
        }
    }

    void CreateBullet()
    {
        Instantiate(bullet, shootPOS.position, shootPOS.rotation);
    }
    IEnumerator displayCooldown()
    {
        gameManager.instance.cooldownRing.SetActive(false);
        if (audioSource != null && snapSound != null)
        {
            audioSource.PlayOneShot(snapSound);
        }
        yield return new WaitForSeconds(fireCooldown);
        gameManager.instance.cooldownRing.SetActive(true);
    }

    public void EnableSlingshot()
    {
        enabled = true;
    }
    public void DisableSlingshot()
    {
        enabled = false;
    }

    public void EnableAudio()
    {
        audioSource.enabled = true;
    }
    public void DisableAudio()
    {
        audioSource.enabled = false;
    }
}