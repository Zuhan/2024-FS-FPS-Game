using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TMP_Dropdown graphicsDropdown;
    public Slider masterVol, musicVol, sfxVol;
    public AudioMixer mainMixer;
    // Start is called before the first frame update
    public void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetQualityLevel()
    {
        QualitySettings.SetQualityLevel(graphicsDropdown.value);
    }
    public void ChangeMasterVolume()
    {
        mainMixer.SetFloat("Master Volume", masterVol.value);
    }
    public void ChangeMusicVolume()
    {
        mainMixer.SetFloat("Music Volume", musicVol.value);
    }
    public void ChangeSFXVolume()
    {
        mainMixer.SetFloat("SFX Volume", sfxVol.value);
    }
    public void PlayDemo()
    {
        SceneManager.LoadSceneAsync(12);
    }
}
