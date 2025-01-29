using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogsPanel : MonoBehaviour
{
    [SerializeField] private GameObject logPrefab;
    [SerializeField] private RectTransform logsHolder;

    private List<GameObject> logInstances = new List<GameObject>();
    private List<LogEventData> logsData = new List<LogEventData>();

    private Color LogColor(LogType type)
    {
        switch (type)
        {
            case LogType.INFO:
                return Color.white;
            case LogType.WARNING:
                return Color.red;
            case LogType.EVENT:
                return Color.cyan;
            case LogType.ALERT:
                return Color.yellow;
            default:
                return Color.white;
        }
    }

    public void EmitLog(LogType type, string content, LogEventData logData = null) 
    {
        GameObject gO = Instantiate(logPrefab, logsHolder).gameObject;
        TMP_Text tmp = gO.GetComponentInChildren<TMP_Text>();
        Image img = gO.GetComponent<Image>();
        if(logData) logData.timeEmited = DateTime.Now.ToString("HH:mm:ss");
        tmp.text = DateTime.Now.ToString("HH:mm:ss") + " - " + Enum.GetName(typeof(LogType), type) + " - " + content;
        img.color = LogColor(type);
        logInstances.Add(gO);
        logsData.Add(logData);
    }

    public void EmitPerformanceLog(string time, LogType type, string content)
    {
        GameObject gO = Instantiate(logPrefab, logsHolder).gameObject;
        TMP_Text tmp = gO.GetComponentInChildren<TMP_Text>();
        tmp.text = "<#4D4D4D>" + time + "</color>" + " - " + Enum.GetName(typeof(LogType), type) + " - " + content;
    }

    public void UpdateLogsLanguage()
    {
        for (int i = 0; i < logInstances.Count - 1; i++)
        {
            TMP_Text tmp = logInstances[i].GetComponentInChildren<TMP_Text>();
            tmp.text = DateTime.Now.ToString("HH:mm:ss") + " - " + Enum.GetName(typeof(LogType), logsData[i].logType) + " - " + logsData[i].localeLabel();
        }
    }

}

public enum LogType
{
    INFO,
    WARNING,
    EVENT,
    ALERT
}

public enum LogEventType
{
    //According to the event type when sending the log it'd do something different
    NONE,
    TYPE1,
    TYPE2,
    TYPE3
}
