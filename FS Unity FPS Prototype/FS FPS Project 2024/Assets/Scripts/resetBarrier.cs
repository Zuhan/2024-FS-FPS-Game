using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetBarrier : MonoBehaviour
{
    [SerializeField] oneWayPlatform[] platforms;
    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < platforms.Length; i++)
        {
            platforms[i].resetPosition();
        }
    }
}
