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
    [SerializeField] public GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] public GameObject cheatInput;
    [SerializeField] List<Image> weaponIcons;
    [SerializeField] TMP_Text potionAmount;

    [Header("----- UI Stuff -----")]
    public Image playerHPBar;
    public Image playerStaminaPool;
    public Image slingShot;
    public Image fireStaff;
    public Image thunderHammer;
    public TMP_Text pointsText;
    public TMP_Text pointsCostText;
    public GameObject playerDamageScreen;
    public GameObject playerHealScreen;

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



    //point count
    public int points;

    public bool isPaused;


    public GameObject playerSpawnPos;

    public bool slingUI;
    public bool fireUI;
    public bool thUI;
    public bool exploUI;
    public bool potionUI;
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
        fireUI = playerStats.fireUI;
        thUI = playerStats.thUI;
        exploUI = playerStats.expStaffUI;
        potionUI = playerStats.potionUI;
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
        if (exploUI)
        {
            ShowWeaponIcon(3);
        }
        if (potionUI)
        {
            ShowWeaponIcon(4);
            potionAmount.text = playerStats.potions.Count.ToString();
            potionAmount.gameObject.SetActive(true);
            potionAmount.enabled = true;
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
        playerStats.money = points;
    }
    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
    public class PlayerData : ScriptableObject
    {
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
    }
    public void ShowWeaponIcon(int weaponIndex)
    {
        if(weaponIndex >= weaponIcons.Count)
        {
            weaponIndex = 0;
        }
        weaponIcons[weaponIndex].enabled = true;
        if(weaponIndex == 0)
        {
            playerStats.slingUI = true;
        }
        else if (weaponIndex == 1)
        {
            playerStats.fireUI = true;
        }
        else if(weaponIndex == 2)
        {
            playerStats.thUI = true;
        }
        else if (weaponIndex == 3)
        {
            playerStats.expStaffUI = true;
        }
        else if(weaponIndex == 4)
        {
            playerStats.potionUI = true;
            potionAmount.text = playerStats.potions.Count.ToString();
            potionAmount.gameObject.SetActive(true);
            potionAmount.enabled = true;
        }
        else
        {
            //last weapon slot
        }
    }
    public void updatePotionUi()
    {
        potionAmount.text = playerStats.potions.Count.ToString();
    }
}
