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
    [SerializeField] public GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] public GameObject cheatInput;
    [SerializeField] List<Image> weaponIcons;

    [Header("----- UI Stuff -----")]
    public Image playerHPBar;
    public Image playerStaminaPool;
    public Image slingShot;
    public Image fireStaff;
    public Image thunderHammer;
    public TMP_Text enemyCountText;
    public TMP_Text pointsText;
    public TMP_Text pointsCostText;
    public GameObject playerDamageScreen;
    public TMP_Text collectibleText;
    public TMP_Text potionCount;
    public GameObject inventory;
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


    //enemy count field 
    public int enemyCount;

    int winPoints = 0;

    //point count
    public int points;

    public bool isPaused;

    public TMP_Text waveText;

    public GameObject playerSpawnPos;
    public bool invOpen;

    public bool slingUI;
    public bool fireUI;
    public bool thUI;
    public bool exploUI;
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
        points = playerStats.money;
        pointsText.text = points.ToString("F0");
        slingUI = playerStats.slingUI;
        //Debug.Log(playerStats.slingUI);
        fireUI = playerStats.fireUI;
        thUI = playerStats.thUI;
        if (slingUI)
        {
            ShowWeaponIcon(0);
        }
        if (fireUI)
        {
            ShowWeaponIcon(1);
        }
        if (thUI)
        {
            ShowWeaponIcon(2);
        }
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
        isPaused = false;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (menuActive != null)
        {
            menuActive.SetActive(false);
        }
        menuActive = null;
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
    public void showInventory()
    {
        menuActive = inventory;
        menuActive.SetActive(true);
        potionCount.text = playerStats.potions.Count.ToString();
        invOpen = true;
        statePaused();
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
        playerStats.money = points;
    }
    public void updateWave(int wave)
    {
        waveText.text = wave.ToString("F0");
    }

    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
    public class PlayerData : ScriptableObject
    {
        //public int points;
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
                invOpen = false;
            }
            else if (menuActive == menuPause || menuActive == inventory)
            {
                stateUnpaused();
                invOpen = false;
            }
        }      
    }

    public void ShowWeaponIcon(int weaponIndex)
    {
        weaponIcons[weaponIndex].enabled = true;
        if(weaponIndex == 0)
        {
            
            playerStats.slingUI = true;
            Debug.Log(playerStats.slingUI);
        }
        else if (weaponIndex == 1)
        {
            playerStats.fireUI = true;
        }
        else
        {
            playerStats.thUI = true;
        }
    }
}
