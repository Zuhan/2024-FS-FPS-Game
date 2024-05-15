using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void resume()
    {
        gameManager.instance.stateUnpaused();
    }
    public void respawn()
    {
        gameManager.instance.playerScript.spawnPlayer();
        gameManager.instance.stateUnpaused();
    }
    public void restart()
    {
        SceneManager.LoadScene("Main Menu");
        playerStats.money = 0;
        playerStats.hp = playerStats.maxHP;
        playerStats.keys.Clear();
        playerStats.weapons.Clear();
        playerStats.potions.Clear();
        playerStats.slingUI = false;
        playerStats.fireUI = false;
        playerStats.thUI = false;
        gameManager.instance.stateUnpaused();
    }
    public void quit()
    {
        Application.Quit();
    }
}
