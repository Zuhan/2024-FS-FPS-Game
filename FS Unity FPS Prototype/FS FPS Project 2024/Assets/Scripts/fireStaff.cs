using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireStaff : MonoBehaviour
{

    public GameObject fireMagicPrefab;
    [SerializeField] Transform shootPos;
    [SerializeField] float castRate;

    bool isCasting;

    // Update is called once per frame
    void Update()
    {
        // Check for left mouse button click to cast fire magic
        if (Input.GetMouseButtonDown(0))
        {
            // Cast fire magic
            CastFireMagic();
        }
    }

    void CastFireMagic()
    {
        // Calculate firing direction based on player's camera orientation
        Vector3 firingDirection = Camera.main.transform.forward;

        // Instantiate the fire magic prefab with the adjusted firing direction
        GameObject fireMagic = Instantiate(fireMagicPrefab, shootPos.position, Quaternion.identity);

        // Store the firing direction as the initial rotation of the fireball
        Quaternion initialRotation = Quaternion.LookRotation(firingDirection);

        // Rotate the fire magic to face the firing direction
        fireMagic.transform.rotation = initialRotation;

        // Ensure the fire magic is not affected by any parent's rotation
        fireMagic.transform.parent = null;

        // Store the initial rotation in the fireball's script for reference
        fireMagic.GetComponent<fireMagic>().SetInitialRotation(initialRotation);
    }

    IEnumerator cast()
    {
        isCasting = true;

        // Instantiate the fire magic prefab at the shoot position
        Instantiate(fireMagicPrefab, shootPos.position, transform.rotation);

        yield return new WaitForSeconds(castRate);
        isCasting = false;
    }
}
