using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //game manager instance
    public static GameManager Instance;

    //game object player
    public GameObject player;

    //void awake so its called first 
    void Awake()
    {
        //instance set to this
        Instance = this;
        //find player object with tag Player
        player = GameObject.FindWithTag("Player");
    }


    void Update()
    {
        


    }



}
