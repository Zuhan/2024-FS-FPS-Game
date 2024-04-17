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


    //Fire Staff prefab added by Derek
    public GameObject fireStaffPrefab;
    //Reference to the fire staff object added by Derek
    private GameObject fireStaffObject;
    //Reference to the fire staff script added by Derek
    private fireStaff fireStaffScript;
    //Fire Magic prefab added by Derek
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

    //Method for equipping Fire_Staff prefab added by Derek
    public void EquipFireStaff()
    {
        if (fireStaffObject == null)
        {
            fireStaffObject = Instantiate(fireStaffPrefab, Vector3.zero, Quaternion.identity);

            fireStaffObject.transform.parent = Camera.main.transform;

            Vector3 offset = new Vector3(0.5f, -0.088f, 0.98f); // X offset, Y offset, Z offset

            fireStaffObject.transform.localPosition = offset;

            Quaternion rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);

            rotation *= Quaternion.Euler(130f, 180f, 0f);

            fireStaffObject.transform.rotation = rotation;

            fireStaffScript = fireStaffObject.GetComponent<fireStaff>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Equipting the Fire Staff by pressing the 1 key added by Derek
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (fireStaffObject != null)
            {
                UnequipFireStaff();
            }
            else
            {
                EquipFireStaff();
            }
        }
    }

    //Unequipping the Fire Staff by pressing the 1 key added by Derek
    public void UnequipFireStaff()
    {
        if (fireStaffObject != null)
        {
            Destroy(fireStaffObject);

            fireStaffObject = null;
            fireStaffScript = null;
        }
    }
}
