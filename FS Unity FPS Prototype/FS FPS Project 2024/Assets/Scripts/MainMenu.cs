using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public TMP_Dropdown graphicsDropdown;
    public Slider masterVol, musicVol, sfxVol, lookSensitivity;
    public AudioMixer mainMixer;
    public GameObject cameraA;
    public BenCamera cameraController;

    float sensitivity;
    // Start is called before the first frame update
    public void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        
        lookSensitivity.value = playerStats.sens;

        masterVol.value = playerStats.masterVol;
        musicVol.value = playerStats.musicVol;
        sfxVol.value = playerStats.sfxVol;
        cameraA = GameObject.FindWithTag("MainCamera");
        cameraController = cameraA.GetComponent<BenCamera>();

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
        playerStats.masterVol = masterVol.value;
    }
    public void ChangeMusicVolume()
    {
        mainMixer.SetFloat("Music Volume", musicVol.value);
        playerStats.musicVol = musicVol.value;
    }
    public void ChangeSFXVolume()
    {
        mainMixer.SetFloat("SFX Volume", sfxVol.value);
        playerStats.sfxVol = sfxVol.value;
    }
    public void PlayDemo()
    {
        SceneManager.LoadSceneAsync(12);
    }
    public void ChangeLookSensitivity()
    {
        //float sensitivity = cameraController.GetSensitivity();
        sensitivity = lookSensitivity.value;
        playerStats.sens = sensitivity;
        if (cameraController != null)
        {
            cameraController.SetSensitivity(sensitivity);
        }
    }
}
