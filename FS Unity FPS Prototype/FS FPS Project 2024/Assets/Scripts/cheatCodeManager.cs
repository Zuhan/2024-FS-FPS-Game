using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class cheatCodeManager : MonoBehaviour
{
    public static cheatCodeManager instance;

    public List<cheatCode> cheatCodes = new List<cheatCode>();
    private string currentInput = "";
    public TMP_InputField inputField;

    public gameManager gameManager;

    private bool cheatActivated = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        inputField.gameObject.SetActive(false);
        inputField.onEndEdit.AddListener(SubmitInput);
        cheatCodeManager.instance.cheatCodes.Add(new cheatCode("pointsoplenty", ActivatePointsOPlenty));
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            CheckCheatCode();
            ResumeGame();
        }
        else
        {
            cheatActivated = false;
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        inputField.gameObject.SetActive(true);
        inputField.Select();
        inputField.ActivateInputField();
    }

    void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    void SubmitInput(string input)
    {
        currentInput = input.ToLower();
        CheckCheatCode();
        inputField.gameObject.SetActive(false);
    }

    void CheckCheatCode()
    {
        foreach (cheatCode cheat in cheatCodes)
        {
            if (cheat != null && currentInput == cheat.code)
            {
                cheat.action.Invoke();
                break;
            }
        }
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
