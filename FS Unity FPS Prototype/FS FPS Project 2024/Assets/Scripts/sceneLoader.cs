using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}