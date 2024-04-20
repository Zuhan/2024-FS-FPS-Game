using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionStaff : MonoBehaviour
{
    //I CALL UPON THE STRONGEST OF MAGIC, FROM THE DARKEST OF DARKNESS AND THE BRIGHTEST OF LIGHTS, TO CAST UPON MY ENEMIES, EXPLOSION!!!!!!!!!!!!!!!!

    public GameObject explosionMagicPrefab;
    [SerializeField] Transform shootPos;
    [SerializeField] float castRate;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CastExplosionMagic();
        }
    }

    void CastExplosionMagic()
    {
        Vector3 firingDirection = Camera.main.transform.forward;

        GameObject explosionMagic = Instantiate(explosionMagicPrefab, shootPos.position, Quaternion.identity);

        Quaternion initialRotation = Quaternion.LookRotation(firingDirection);

        explosionMagic.transform.rotation = initialRotation;

        explosionMagic.transform.parent = null;

        explosionMagic.GetComponent<fireMagic>().SetInitialRotation(initialRotation);
    }
}