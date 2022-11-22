using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusBar : Singleton<StatusBar>
{
    public TextMeshProUGUI statusMessage;

    public static void Print(string message)
    {
        Instance.statusMessage.text = message;
    }
}
