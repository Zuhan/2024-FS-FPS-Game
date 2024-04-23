using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireStaff : MonoBehaviour, ICooldownWeapon
{
    //MY STAFF OF FIRE. I, DEREK, CALL UPON THEE TO BURN MY ENEMIES TO ASHES

    public GameObject fireMagicPrefab;
    [SerializeField] Transform shootPos;
    [SerializeField] private float fireCooldown = 0.5f;
    private float lastFireTime;

    // Properties implementing the ICooldownWeapon interface
    public float FireCooldown { get { return fireCooldown; } }
    public float LastFireTime { get { return lastFireTime; } }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time - lastFireTime > fireCooldown)
        {
            CastFireMagic();
            lastFireTime = Time.time;
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
}