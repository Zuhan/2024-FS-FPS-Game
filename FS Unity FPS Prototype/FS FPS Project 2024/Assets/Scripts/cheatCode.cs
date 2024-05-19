using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cheatCode
{
    public string code;
    public Action action;
    public Action<string> actionWithString;

    public cheatCode(string _code, Action _action)
    {
        code = _code;
        action = _action;
    }

    public cheatCode(string _code, Action<string> _actionWithString)
    {
        code = _code;
        actionWithString = _actionWithString;
    }
}