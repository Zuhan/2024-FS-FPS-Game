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
    [SerializeField] private GameObject explosionStaff;

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
    {if (!cheatActivated)
        {
            Vector3 dropPosition = player.transform.position + player.transform.forward * 4;
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
                case "explosionstaff":
                    itemPrefab = explosionStaff;
                    break;
                default:
                    Debug.LogWarning("Item not found!");
                    return;
            }
            Instantiate(itemPrefab, dropPosition, Quaternion.identity);
            cheatActivated = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            inputField.gameObject.SetActive(true);
            inputField.Select();
            inputField.ActivateInputField();
            gameManager.instance.menuActive = gameManager.instance.cheatInput;
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

    void SubmitInput(string input)
    {
        currentInput = input.ToLower();
        CheckCheatCode();
        inputField.gameObject.SetActive(false);
    }

    void CheckCheatCode()
    {
        string[] inputParts = currentInput.Trim().Split(' ');
        string cheatCode = inputParts[0];
        foreach (cheatCode cheat in cheatCodes)
        {
            if (cheat != null && cheatCode == cheat.code)
            {
                if (inputParts.Length > 1)
                {
                    string cheatArgument = inputParts[1];

                    if (cheat.actionWithString != null)
                    {
                        cheat.actionWithString.Invoke(cheatArgument);
                    }
                    else
                    {
                        Debug.LogWarning("Cheat code does not require an argument!");
                    }
                }
                else
                {
                    if (cheat.action != null)
                    {
                        cheat.action.Invoke();
                    }
                    else
                    {
                        Debug.LogWarning("Cheat code does not require an argument!");
                    }
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