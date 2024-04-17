using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magicController : MonoBehaviour
{
    public GameObject FireMagicPrefab;
    public GameObject WindMagicPrefab;
    public GameObject EarthMagicPrefab;
    public GameObject ElectricMagicPrefab;
    public GameObject WaterMagicPrefab;

    public void CastFireMagic(Vector3 targetPosition)
    {
        GameObject fireMagic = Instantiate(FireMagicPrefab, targetPosition, Quaternion.identity);
    }
    public void CastWindMagic(Vector3 targetPosition)
    {
        GameObject windMagic = Instantiate(WindMagicPrefab, targetPosition, Quaternion.identity);
    }

    public void CastEarthMagic(Vector3 targetPosition)
    {
        GameObject earthMagic = Instantiate(EarthMagicPrefab, targetPosition, Quaternion.identity);
    }

    public void CastElectricMagic(Vector3 targetPosition)
    {
        GameObject electricMagic = Instantiate(ElectricMagicPrefab, targetPosition, Quaternion.identity);
    }

    public void CastWaterMagic(Vector3 targetPosition)
    {
        GameObject waterMagic = Instantiate(WaterMagicPrefab, targetPosition, Quaternion.identity);
    }
}
