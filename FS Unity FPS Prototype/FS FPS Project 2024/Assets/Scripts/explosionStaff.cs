using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionStaff : MonoBehaviour
{
    ////I CALL UPON THE STRONGEST OF MAGIC, FROM THE DARKEST OF DARKNESS AND THE BRIGHTEST OF LIGHTS, TO CAST UPON MY ENEMIES, EXPLOSION!!!!!!!!!!!!!!!!

    public GameObject explosionMagicPrefab;
    [SerializeField] Transform shootPos;
    [SerializeField] private float fireCooldown = 0.5f;
    [SerializeField] GameObject crystal;
    private float lastFireTime;

    void Start()
    {
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            if (Input.GetMouseButtonDown(0) && Time.time - lastFireTime > fireCooldown)
            {
                CastExplosionMagic();
                lastFireTime = Time.time;
                StartCoroutine(displayCooldown());
            }
        }
    }

    void CastExplosionMagic()
    {
        Vector3 firingDirection = Camera.main.transform.forward;

        GameObject explosionMagic = Instantiate(explosionMagicPrefab, shootPos.position, Quaternion.identity);

        Quaternion initialRotation = Quaternion.LookRotation(firingDirection);

        explosionMagic.transform.rotation = initialRotation;

        explosionMagic.transform.parent = null;

        explosionMagic.GetComponent<explosionMagic>().SetInitialRotation(initialRotation);
    }

    IEnumerator displayCooldown()
    {
        gameManager.instance.cooldownRing.SetActive(false);
        yield return new WaitForSeconds(fireCooldown);
        gameManager.instance.cooldownRing.SetActive(true);
    }

    public void EnableExplosionStaff()
    {
        enabled = true;
        crystal.SetActive(true );
    }
    public void DisableExplosionStaff()
    {
        enabled = false;
        crystal.SetActive(false );
    }
}
