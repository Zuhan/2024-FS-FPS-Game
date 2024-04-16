using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    //field for the interact text UI
    [SerializeField] GameObject interactText;
    [SerializeField] GameObject interactFailText;

    //game manager instance
    public static gameManager instance;

    //game object player
    public GameObject player;

    //MagicController added by Derek
    public magicController magicController;

    // Fire Staff prefab added by Derek
    public GameObject fireStaffPrefab;

    // Reference to the fire staff object added by Derek
    private GameObject fireStaffObject;

    // Reference to the fire staff script added by Derek
    private fireStaff fireStaffScript;

    // Fire Magic prefab added by Derek
    public GameObject FireMagicPrefab;

    //enemy count field 
    public int enemyCount;

    //point count
    public int points;

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
            Debug.LogError("magicController not found in scene");
        }

        // Instantiate the Fire Staff prefab in the scene
        fireStaffObject = Instantiate(fireStaffPrefab, Vector3.zero, Quaternion.identity);
        fireStaffObject.SetActive(false); // Hide the fire staff initially

        // Get the fireStaff script from the player object
        fireStaffScript = player.GetComponent<fireStaff>();
        if (fireStaffScript == null)
        {
            // If the fireStaff script is not already attached to the player, add it
            fireStaffScript = player.AddComponent<fireStaff>();
        }
        //setting points
        points = 0;
    }


    void Update()
    {
        // Check for player input to pick up the fire staff
        if (Input.GetKeyDown(KeyCode.E))
        {
            PickupFireStaff();
        }
    }

    // Method for picking up the fire staff
    void PickupFireStaff()
    {
        // Activate the fire staff object in the scene
        fireStaffObject.SetActive(true);

        // Assign the fire magic prefab to the fireStaff script
        fireStaffScript.fireMagicPrefab = FireMagicPrefab;
    }


    //method for adding and removing enemy count
    public void updateGameGoal(int count)
    {
        enemyCount += count;
    }

    //method for displaying interact menu
    public void showInteractText()
    {
        interactText.SetActive(true);
    }
    //method for hiding interact text
    public void hideInteractText()
    {
        interactText?.SetActive(false);
    }
    //method for showing when you dont have enough points
    public void showInteractFail()
    {
        interactFailText.SetActive(true);
    }
    //method getting rid of that text
    public void hideInteractFail()
    {
        interactFailText?.SetActive(false);
    }
    public void pointsChange(int amount)
    {
        points += amount;
    }
}
