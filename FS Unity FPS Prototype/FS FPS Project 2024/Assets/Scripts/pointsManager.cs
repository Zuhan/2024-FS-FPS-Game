using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PointsManager : MonoBehaviour
{
    public static PointsManager Instance;

    public delegate void PointChangeHandeler(int amount);
    public event PointChangeHandeler OnPointChange;


    //Singleton Check
    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void AddPoints(int amount)
    {
        OnPointChange?.Invoke(amount);
    }
}
