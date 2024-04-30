using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireStaff : MonoBehaviour
{
    //MY STAFF OF FIRE. I, DEREK, CALL UPON THEE TO BURN MY ENEMIES TO ASHES

    public GameObject fireMagicPrefab;
    [SerializeField] Transform shootPos;
    [SerializeField] private float fireCooldown = 0.5f;
    private float lastFireTime;


    void Start()
    {
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time - lastFireTime > fireCooldown)
        {
            CastFireMagic();
            lastFireTime = Time.time;
            StartCoroutine(displayCooldown());
        }
    }

    void CastFireMagic()
    {
        Vector3 firingDirection = Camera.main.transform.forward;

        GameObject fireMagic = Instantiate(fireMagicPrefab, shootPos.position, Quaternion.identity);

        Quaternion initialRotation = Quaternion.LookRotation(firingDirection);

        fireMagic.transform.rotation = initialRotation;

        fireMagic.transform.parent = null;

        fireMagic.GetComponent<fireMagic>().SetInitialRotation(initialRotation);
    }

    IEnumerator displayCooldown()
    {
        gameManager.instance.cooldownRing.SetActive(true);
        yield return new WaitForSeconds(fireCooldown);
        gameManager.instance.cooldownRing.SetActive(false);
    }

    public void EnableFireStaff()
    {
        enabled = true;
    }
}