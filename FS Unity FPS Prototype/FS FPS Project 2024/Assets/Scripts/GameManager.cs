using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gameManager : MonoBehaviour
{
    //fields for the UI 
    [SerializeField] GameObject interactText;
    [SerializeField] GameObject interactFailText;
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;

    //public fields for more UI
    public Image playerHPBar;
    public TMP_Text enemyCountText;
    public TMP_Text pointsText;
    public TMP_Text pointsCostText;
    public GameObject playerDamageScreen;

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
    //Fire Staff prefab added by Derek
    public GameObject explosionStaffPrefab;
    //Reference to the fire staff object added by Derek
    private GameObject explosionStaffObject;
    //Reference to the fire staff script added by Derek
    private explosionStaff explosionStaffScript;
    //Fire Magic prefab added by Derek
    public GameObject ExplosionMagicPrefab;


    //enemy count field 
    public int enemyCount;

    //point count
    public int points;

    public bool isPaused;
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
    public void statePaused()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void stateUnpaused()
    {
        isPaused = !isPaused;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState= CursorLockMode.Locked;
        menuActive.SetActive(isPaused);
        menuActive = null;
    }
    //method for adding and removing enemy count and determining if you win
    public void updateGameGoal(int count)
    {
        enemyCount += count;
        enemyCountText.text = enemyCount.ToString("F0");
        if(enemyCount <= 0)
        {
            statePaused();
            menuActive = menuWin;
            menuActive.SetActive(isPaused);
        }
    }
    //method for losing
    public void lose()
    {
        statePaused();
        menuActive = menuLose;
        menuActive.SetActive(true);
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
        pointsText.text = points.ToString("F0");
    }

    // Method for equipping any type of weapon after ensuring no other weapon is already equipped
    private void EquipWeaponSafely(GameObject weaponPrefab, Vector3 offset, Quaternion rotation)
    {
        if (Camera.main.transform.childCount > 0)
        {
            // Destroy the currently equipped weapon
            Destroy(Camera.main.transform.GetChild(0).gameObject);
        }

        // Instantiate and equip the new weapon
        GameObject equippedWeapon = Instantiate(weaponPrefab, Vector3.zero, Quaternion.identity);
        equippedWeapon.transform.parent = Camera.main.transform;
        equippedWeapon.transform.localPosition = offset;
        equippedWeapon.transform.rotation = rotation;
    }

    //Method for equipping Fire_Staff prefab added by Derek
    public void EquipFireStaff()
    {
        if (fireStaffObject == null)
        {
            Vector3 offset = new Vector3(0.5f, -0.088f, 0.98f); // X offset, Y offset, Z offset

            Quaternion rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
            rotation *= Quaternion.Euler(130f, 180f, 0f);

            EquipWeaponSafely(fireStaffPrefab, offset, rotation);

            fireStaffObject = Camera.main.transform.GetChild(0).gameObject;
            fireStaffScript = fireStaffObject.GetComponent<fireStaff>();
        }
        else
        {
            UnequipFireStaff();
        }
    }

    //Method for equipping Explosion_Staff prefab added by Derek
    public void EquipExplosionStaff()
    {
        if (explosionStaffObject == null)
        {
            Vector3 offset = new Vector3(0.5f, -1.2f, -0.3f); // X offset, Y offset, Z offset

            Quaternion rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
            rotation *= Quaternion.Euler(-45f, 180f, 0f);

            EquipWeaponSafely(explosionStaffPrefab, offset, rotation);

            explosionStaffObject = Camera.main.transform.GetChild(0).gameObject;
            explosionStaffScript = explosionStaffObject.GetComponent<explosionStaff>();
        }
        else
        {
            UnequipExplosionStaff();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && menuActive == null)
        {
            statePaused();
            menuActive = menuPause;
            menuActive.SetActive(isPaused);
        }
        //Equipping the Fire Staff by pressing the 1 key added by Derek
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
        //Equipping the Explosion_Staff by pressing 0 key
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (explosionStaffObject != null)
            {
                UnequipExplosionStaff();
            }
            else
            {
                EquipExplosionStaff();
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
    
    //Unequipping the Explosion_Staff by pressing the 1 key added by Derek
    public void UnequipExplosionStaff()
    {
        if (explosionStaffObject != null)
        {
            Destroy(explosionStaffObject);

            explosionStaffObject = null;
            explosionStaffScript = null;
        }
    }
}
