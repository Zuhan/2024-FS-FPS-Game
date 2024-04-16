using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PointsManager : MonoBehaviour
{
    public static PointsManager Instance;

    public delegate void PointChangeHandler(int amount);
    public event PointChangeHandler OnPointChange;

    

    //Singleton Check
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPoints(int amount)
    {
        Debug.Log("AddPoints called"); 
        OnPointChange?.Invoke(amount);
    }
}
