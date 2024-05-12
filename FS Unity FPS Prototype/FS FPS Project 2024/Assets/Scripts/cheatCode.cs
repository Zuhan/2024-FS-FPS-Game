using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cheatCode
{
    public string code;
    public Action action; // Action without arguments
    public Action<string> actionWithString; // Action with a string argument

    // Constructor for methods without arguments
    public cheatCode(string _code, Action _action)
    {
        code = _code;
        action = _action;
    }

    // Constructor for methods with a string argument
    public cheatCode(string _code, Action<string> _actionWithString)
    {
        code = _code;
        actionWithString = _actionWithString;
    }
}