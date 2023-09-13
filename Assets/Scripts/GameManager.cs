using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private Debugger debug;

    private void Start()
    {
        debug = GetComponent<Debugger>();
    }

    public void DebugLog(string message)
    {
        debug.DebugLog(message + "\n");
    }
}
