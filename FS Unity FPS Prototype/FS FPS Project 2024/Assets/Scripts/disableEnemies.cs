using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableEnemies : MonoBehaviour
{
    [SerializeField] GameObject enemies;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemies.SetActive(false);
        }
    }
}