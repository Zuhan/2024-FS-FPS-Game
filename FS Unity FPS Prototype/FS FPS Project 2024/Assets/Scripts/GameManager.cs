using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;
using UnityEditor;

public class gameManager : MonoBehaviour
{
    [Header("----- Menu Stuff -----")]
    [SerializeField] GameObject interactText;
    [SerializeField] GameObject interactFailText;
    [SerializeField] GameObject barricadeText;
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;

    [Header("----- UI Stuff -----")]
    public Image playerHPBar;
    public TMP_Text enemyCountText;
    public TMP_Text pointsText;
    public TMP_Text pointsCostText;
    public GameObject playerDamageScreen;
    public TMP_Text collectibleText;
    //public List<GameObject> enemies;
    //public GameObject spawner;

    //game manager instance
    public static gameManager instance;

    //game object player and playercontroller
    public GameObject player;
    public playerController playerScript;

    [Header("----- Fire Staff -----")]
    //Fire Staff prefab added by Derek
    public GameObject fireStaffPrefab;
    //Reference to the fire staff object added by Derek
    private GameObject fireStaffObject;
    //Reference to the fire staff script added by Derek
    private fireStaff fireStaffScript;
    //Fire Magic prefab added by Derek
    public GameObject FireMagicPrefab;
    //Cooldown Ring Added by Derek
    public GameObject cooldownRing;

    //Explosion Being Shelved
    ////Explosion Staff prefab added by Derek
    //public GameObject explosionStaffPrefab;
    ////Reference to the explosion staff object added by Derek
    //private GameObject explosionStaffObject;
    ////Reference to the explosion staff script added by Derek
    //private explosionStaff explosionStaffScript;
    ////Explosion Magic prefab added by Derek
    //public GameObject ExplosionMagicPrefab;
    ////Nuclear Explosion prefab added by Derek
    //public GameObject Explosion;


    //enemy count field 
    public int enemyCount;

    int winPoints = 0;

    //point count
    public int points;

    public bool isPaused;

    public TMP_Text waveText;

    public GameObject playerSpawnPos;

    //void awake so its called first 
    void Awake()
    {
        //instance set to this
        instance = this;
        //find player object with tag Player and player controller
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");
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
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(isPaused);
        menuActive = null;
    }
    //method for adding and removing enemy count and determining if you win
    public void updateGameGoal(int count)
    {
        enemyCount += count;
        enemyCountText.text = enemyCount.ToString("F0");
        if (enemyCount <= 0 && waveManager.instance.waveCurrent >= waveManager.instance.spawners.Length)
        {
            statePaused();
            menuActive = menuWin;
            menuActive.SetActive(isPaused);
        }
    }

    public void win()
    {
        statePaused();
        menuActive = menuWin;
        menuActive.SetActive(isPaused);
    }
    public void winByPoints()
    {
        winPoints++;
        collectibleText.text = winPoints.ToString("F0");
        if (winPoints >= 5)
        {
            win();
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
    public void showInteractText(int cost)
    {
        pointsCostText.text = cost.ToString("F0");
        interactText.SetActive(true);
    }
    public void showBarricadeText(int cost)
    {
        pointsCostText.text = cost.ToString("F0");
        barricadeText.SetActive(true);
    }

    //method for hiding interact text
    public void hideInteractText()
    {
        interactText?.SetActive(false);
    }
    public void hideBarricadeText()
    {    
        barricadeText?.SetActive(false);
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
    public void updateWave(int wave)
    {
        waveText.text = wave.ToString("F0");
    }

    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
    public class PlayerData : ScriptableObject
    {
        public int points;
        public List<weaponStats> weapons = new List<weaponStats>();
        // Add other player data here
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePaused();
                menuActive = menuPause;
                menuActive.SetActive(isPaused);
            }
            else if (menuActive == menuPause)
            {
                stateUnpaused();
            }
        }

        //Equipping the Fire Staff by pressing the 1 key added by Derek
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    if (fireStaffObject != null)
        //    {
        //        UnequipFireStaff();
        //    }
        //    else
        //    {
        //        EquipFireStaff();
        //    }
        //}

        //Explosion Staff Being Shelved
        //Equipping the Explosion_Staff by pressing 0 key
        //if (Input.GetKeyDown(KeyCode.Alpha0))
        //{
        //    if (explosionStaffObject != null)
        //    {
        //        UnequipExplosionStaff();
        //    }
        //    else
        //    {
        //        EquipExplosionStaff();
        //    }
        //}
        
    }

    //Unequipping the Fire Staff by pressing the 1 key added by Derek
    //public void UnequipFireStaff()
    //{
    //    if (fireStaffObject != null)
    //    {
    //        Destroy(fireStaffObject);

    //        fireStaffObject = null;
    //        fireStaffScript = null;
    //    }
    //}


    //Explosion Staff Being Shelved
    //Unequipping the Explosion_Staff by pressing the 1 key added by Derek
    //public void UnequipExplosionStaff()
    //{
    //    if (explosionStaffObject != null)
    //    {
    //        Destroy(explosionStaffObject);

    //        explosionStaffObject = null;
    //        explosionStaffScript = null;
    //    }
    //}
    // Method for equipping any type of weapon after ensuring no other weapon is already equipped
    //private void EquipWeaponSafely(GameObject weaponPrefab, Vector3 offset, Quaternion rotation)
    //{
    //    if (Camera.main.transform.childCount > 0)
    //    {
    //        // Destroy the currently equipped weapon
    //        Destroy(Camera.main.transform.GetChild(0).gameObject);
    //    }

    //    // Instantiate and equip the new weapon
    //    GameObject equippedWeapon = Instantiate(weaponPrefab, Vector3.zero, Quaternion.identity);
    //    equippedWeapon.transform.parent = Camera.main.transform;
    //    equippedWeapon.transform.localPosition = offset;
    //    equippedWeapon.transform.rotation = rotation;
    //}

    //Method for equipping Fire_Staff prefab added by Derek
    //public void EquipFireStaff()
    //{
    //    if (fireStaffObject == null)
    //    {
    //        Vector3 offset = new Vector3(0.5f, -0.088f, 0.98f); // X offset, Y offset, Z offset

    //        Quaternion rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
    //        rotation *= Quaternion.Euler(130f, 180f, 0f);

    //        EquipWeaponSafely(fireStaffPrefab, offset, rotation);

    //        fireStaffObject = Camera.main.transform.GetChild(0).gameObject;
    //        fireStaffScript = fireStaffObject.GetComponent<fireStaff>();
    //    }
    //    else
    //    {
    //        UnequipFireStaff();
    //    }
    //}

    //Explosion Staff Being Shelved
    //Method for equipping Explosion_Staff prefab added by Derek
    //public void EquipExplosionStaff()
    //{
    //    if (explosionStaffObject == null)
    //    {
    //        Vector3 offset = new Vector3(0.5f, -1.2f, -0.3f); // X offset, Y offset, Z offset

    //        Quaternion rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
    //        rotation *= Quaternion.Euler(-45f, 180f, 0f);

    //        EquipWeaponSafely(explosionStaffPrefab, offset, rotation);

    //        explosionStaffObject = Camera.main.transform.GetChild(0).gameObject;
    //        explosionStaffScript = explosionStaffObject.GetComponent<explosionStaff>();
    //    }
    //    else
    //    {
    //        UnequipExplosionStaff();
    //    }
    //}
}
