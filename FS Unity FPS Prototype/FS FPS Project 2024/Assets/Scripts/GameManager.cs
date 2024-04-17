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

        //setting points
        points = 0;
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

    // method for equipping Fire_Staff prefab
    public void EquipFireStaff()
    {
        // Check if Fire Staff prefab is not already equipped
        if (fireStaffObject == null)
        {
            // Instantiate Fire Staff prefab
            fireStaffObject = Instantiate(fireStaffPrefab, Vector3.zero, Quaternion.identity);

            // Set its parent to Main Camera
            fireStaffObject.transform.parent = Camera.main.transform;

            // Calculate offset to position Fire Staff in front of the Main Camera
            Vector3 offset = new Vector3(0.5f, -0.088f, 0.98f); // Adjust this offset as needed

            // Set its position relative to Main Camera
            fireStaffObject.transform.localPosition = offset;

            // Calculate rotation to align Fire Staff with camera's forward direction
            Quaternion rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);

            // Apply a 45-degree rotation around the right axis (assuming staff's initial orientation is aligned with the camera)
            rotation *= Quaternion.Euler(130f, 180f, 0f);

            // Set its rotation
            fireStaffObject.transform.rotation = rotation;

            // Get the fireStaff script component
            fireStaffScript = fireStaffObject.GetComponent<fireStaff>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the number 1 key is pressed
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Check if Fire Staff is already equipped
            if (fireStaffObject != null)
            {
                // Unequip Fire Staff
                UnequipFireStaff();
            }
            else
            {
                // Equip Fire Staff
                EquipFireStaff();
            }
        }
    }

    // method for unequipping Fire_Staff prefab
    public void UnequipFireStaff()
    {
        // Check if Fire Staff is equipped
        if (fireStaffObject != null)
        {
            // Destroy the Fire Staff object
            Destroy(fireStaffObject);

            // Reset reference variables
            fireStaffObject = null;
            fireStaffScript = null;
        }
    }
}
