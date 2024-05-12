using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class cheatCodeManager : MonoBehaviour
{
    [SerializeField] private GameObject slingshot;
    [SerializeField] private GameObject fireStaff;
    [SerializeField] private GameObject thunderHammer;

    public static cheatCodeManager instance;

    public List<cheatCode> cheatCodes = new List<cheatCode>();
    private string currentInput = "";
    public TMP_InputField inputField;

    public gameManager gameManager;
    public GameObject player;
    public playerController playerScript;

    private bool cheatActivated = false;

    private void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
    }

    private void Start()
    {
        inputField.gameObject.SetActive(false);
        inputField.onEndEdit.AddListener(SubmitInput);
        cheatCodeManager.instance.cheatCodes.Add(new cheatCode("pointsoplenty", ActivatePointsOPlenty));
        cheatCodeManager.instance.cheatCodes.Add(new cheatCode("iamgod", ToggleGodMode));
        cheatCodeManager.instance.cheatCodes.Add(new cheatCode("noclip", ToggleNoClipMode));
        cheatCodeManager.instance.cheatCodes.Add(new cheatCode("drop", DropItemCheat));
    }

    void DropItemCheat(string itemName)
    {
        Debug.Log("Item Drop Code Called");
        DropItem(itemName);
    }

    void ActivatePointsOPlenty()
    {
        if (!cheatActivated)
        {
            gameManager.points += 1000;
            gameManager.pointsText.text = gameManager.points.ToString("F0");
            playerStats.money = gameManager.points;
            cheatActivated = true;
        }
    }

    void ToggleGodMode()
    {
        if (!cheatActivated)
        {
            playerController playerCtrl = player.GetComponent<playerController>();
            playerCtrl.godModeActive = !playerCtrl.godModeActive;
            cheatActivated = true;
        }
    }

    void ToggleNoClipMode()
    {
        if (!cheatActivated)
        {
            playerController playerCtrl = player.GetComponent<playerController>();
            playerCtrl.ToggleNoclipMode();
            cheatActivated = true;
        }
    }

    void DropItem(string itemName)
    {
        Debug.Log("Dropping item: " + itemName);
        Vector3 dropPosition = player.transform.position + player.transform.forward * 4; // Adjust distance in front of the player
        GameObject itemPrefab = null;

        switch (itemName.ToLower())
        {
            case "slingshot":
                itemPrefab = slingshot;
                break;
            case "firestaff":
                itemPrefab = fireStaff;
                break;
            case "thunderhammer":
                itemPrefab = thunderHammer;
                break;
            // Add more cases for other items if needed
            default:
                Debug.LogWarning("Item not found!");
                return;
        }

        // Instantiate the item prefab at the calculated drop position
        Instantiate(itemPrefab, dropPosition, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            gameManager.instance.menuActive = gameManager.instance.cheatInput;
            inputField.gameObject.SetActive(true);
            inputField.Select();
            inputField.ActivateInputField();
            gameManager.instance.statePaused();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            CheckCheatCode();
            gameManager.instance.stateUnpaused();
        }
        else
        {
            cheatActivated = false;
        }
    }

    //void PauseGame()
    //{
    //    Time.timeScale = 0f;
    //    inputField.gameObject.SetActive(true);
    //    inputField.Select();
    //    inputField.ActivateInputField();
    //}

    //void ResumeGame()
    //{
    //    Time.timeScale = 1f;
    //}

    void SubmitInput(string input)
    {
        currentInput = input.ToLower();
        CheckCheatCode();
        inputField.gameObject.SetActive(false);
        Debug.Log("Input: " + currentInput);
    }

    void CheckCheatCode()
    {
        string[] inputParts = currentInput.Trim().Split(' ');
        if (inputParts.Length < 2)
        {
            Debug.LogWarning("Invalid cheat code format!");
            inputField.text = "";
            return;
        }

        string cheatCode = inputParts[0];
        string cheatArgument = inputParts[1];

        foreach (cheatCode cheat in cheatCodes)
        {
            if (cheat != null && cheatCode == cheat.code)
            {
                if (cheat.action != null)
                {
                    cheat.action.Invoke();
                }
                else if (cheat.actionWithString != null)
                {
                    cheat.actionWithString.Invoke(cheatArgument);
                }
                inputField.text = "";
                return;
            }
        }

        Debug.LogWarning("Cheat code not found!");
        inputField.text = "";
    }

    bool IsGamePaused()
    {
        return Time.timeScale == 0f;
    }

    public void AddInput(char c)
    {
        currentInput += c;
    }
}