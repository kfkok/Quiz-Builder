using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class UrlParamReader : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void GetJsonFromUrl();

    public OnUrlParamReadEvent OnUrlParamRead;

    // Start is called before the first frame update
    void Start()
    {
        name = "Url Param Reader";
    }

    private void OnValidate()
    {
        name = "Url Param Reader";
    }

    public void Read()
    {
        GetJsonFromUrl();
    }

    public void RecieveJson(string json)
    {
        //print("UrlParamReader.json => " + json);
        OnUrlParamRead?.Invoke(json);
    }
}

[System.Serializable]
public class OnUrlParamReadEvent : UnityEvent<string>
{

}
