using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cheatCode
{
    public string code;
    public System.Action action;

    public cheatCode(string code, System.Action action)
    {
        this.code = code;
        this.action = action;
    }
}