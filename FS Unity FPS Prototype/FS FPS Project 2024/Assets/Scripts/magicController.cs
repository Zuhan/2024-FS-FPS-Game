using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magicController : MonoBehaviour
{

    // Casting Fire Magic
    public void CastFireMagic(Vector3 targetPosition)
    {
        // Instantiate the Fire Magic prefab at the target position
        GameObject fireMagic = Instantiate(FireMagicPrefab, targetPosition, Quaternion.identity);
    }

    // Casting Wind Magic
    public void CastWindMagic(Vector3 targetPosition)
    {
        // Instantiate the Wind Magic prefab at the target position
        GameObject windMagic = Instantiate(WindMagicPrefab, targetPosition, Quaternion.identity);
    }

    // Casting Earth Magic
    public void CastEarthMagic(Vector3 targetPosition)
    {
        // Instantiate the Earth Magic prefab at the target position
        GameObject earthMagic = Instantiate(EarthMagicPrefab, targetPosition, Quaternion.identity);
    }

    // Casting Electric Magic
    public void CastElectricMagic(Vector3 targetPosition)
    {
        // Instantiate the Electric Magic prefab at the target position
        GameObject electricMagic = Instantiate(ElectricMagicPrefab, targetPosition, Quaternion.identity);
    }

    // Casting Water Magic
    public void CastWaterMagic(Vector3 targetPosition)
    {
        // Instantiate the Water Magic prefab at the target position
        GameObject waterMagic = Instantiate(WaterMagicPrefab, targetPosition, Quaternion.identity);
    }

    // Prefabs for each type of magic
    public GameObject FireMagicPrefab;
    public GameObject WindMagicPrefab;
    public GameObject EarthMagicPrefab;
    public GameObject ElectricMagicPrefab;
    public GameObject WaterMagicPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
