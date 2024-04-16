using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{

    //game manager instance
    public static gameManager instance;

    //game object player
    public GameObject player;

    //MagicController added by Derek
    public magicController magicController;

    //enemy count field 
    public int enemyCount;


    //void awake so its called first 
    void Awake()
    {
        //instance set to this
        instance = this;
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


    //method for adding and removing enemy count
    public void updateGameGoal(int count)
    {
        enemyCount += count;
    }

}
