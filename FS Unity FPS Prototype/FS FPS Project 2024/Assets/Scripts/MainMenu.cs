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
    public Slider masterVol, musicVol, sfxVol, lookSensitivity;
    public AudioMixer mainMixer;
    public GameObject camera;
    public BenCamera cameraController;

    float sensitivity;
    // Start is called before the first frame update
    public void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        camera = GameObject.FindWithTag("MainCamera");
        cameraController = camera.GetComponent<BenCamera>();
        sensitivity = cameraController.GetSensitivity();
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
    public void ChangeLookSensitivity()
    {
        //float sensitivity = cameraController.GetSensitivity();
        sensitivity = lookSensitivity.value;
        cameraController.SetSensitivity(sensitivity);
        
    }
}
