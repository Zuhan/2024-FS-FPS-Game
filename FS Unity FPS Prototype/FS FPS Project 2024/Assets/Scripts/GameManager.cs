using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //game manager instance
    public static GameManager Instance;

    //game object player
    public GameObject player;

    //MagicController added by Derek
    public magicController magicController;

    //void awake so its called first 
    void Awake()
    {
        //instance set to this
        Instance = this;
        //find player object with tag Player
        player = GameObject.FindWithTag("Player");

        //MagicController added by Derek
        magicController = FindObjectOfType<magicController>();
        if (magicController == null)
        {
            Debug.LogError("MagicController not found in scene");
        }
    }


    void Update()
    {
        


    }



}
