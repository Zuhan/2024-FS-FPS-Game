using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thunderHammer : MonoBehaviour
{

    public GameObject chainLightningPrefab;
    [SerializeField] Transform castPos;
    [SerializeField] float castRate;
    private float casted;


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time - casted > castRate)
        {
            CastChainLightning();
            casted = Time.time;
        }
    }

    void CastChainLightning()
    {
        GameObject chainLightning = Instantiate(chainLightningPrefab, castPos.position, Quaternion.identity);
        Vector3 castDir = Camera.main.transform.forward;
        Quaternion startRot = Quaternion.LookRotation(castDir);

        chainLightning.transform.rotation = startRot;
        chainLightning.transform.parent = null;

        chainLightning.GetComponent<thunderMagic>().SetInitialRotation(startRot);
    }
}
