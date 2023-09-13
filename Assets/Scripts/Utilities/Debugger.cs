using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Debugger : MonoBehaviour
{
    [SerializeField] public TMP_Text _txtDebug;

    public void DebugLog(string message)
    {
        _txtDebug.text += message;
    }
}
