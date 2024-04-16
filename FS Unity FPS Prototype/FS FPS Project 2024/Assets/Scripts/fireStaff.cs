using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireStaff : MonoBehaviour
{

    public GameObject fireMagicPrefab;

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
        // Instantiate the fire magic prefab
        Instantiate(fireMagicPrefab, transform.position, transform.rotation);
    }
}
