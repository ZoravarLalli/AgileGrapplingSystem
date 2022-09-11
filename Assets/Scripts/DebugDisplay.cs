using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Got this debugger from https://www.youtube.com/watch?v=Pi4SHO0IEQY
// Using this for ease of access while developing and testing in oculus virtual desktop

public class DebugDisplay : MonoBehaviour
{

    Dictionary<string, string> logs = new Dictionary<string, string>();
    public Text display;

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog; // Whenever we get a debug log message it fires off handle
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stack, LogType type)
    {
        // Can add more log types here if needed just make a switch
        if (type == LogType.Log)
        {
            string[] splitString = logString.Split(char.Parse(":"));
            string debugKey = splitString[0];
            string debugValue = splitString.Length > 1 ? splitString[1] : "";

            if (logs.ContainsKey(debugKey))
            {
                logs[debugKey] = debugValue;
            }
            else
            {
                logs.Add(debugKey, debugValue);
            }
        }

        string displayText = "";
        foreach(KeyValuePair<string, string> log in logs)
        {
            if(log.Value == "")
            {
                displayText += log.Key + "\n";
            }
            else
            {
                displayText += log.Key + ": " + log.Value + "\n";
            }
            display.text = displayText;
        }
    }
}
