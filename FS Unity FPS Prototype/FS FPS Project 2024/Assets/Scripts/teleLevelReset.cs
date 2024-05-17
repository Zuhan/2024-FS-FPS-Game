using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleLevelReset : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        gameManager.instance.playerScript.spawnPlayer();
    }
}
