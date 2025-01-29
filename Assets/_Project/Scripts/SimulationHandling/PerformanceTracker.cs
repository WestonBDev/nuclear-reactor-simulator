using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PerformanceTracker : MonoBehaviour
{
    public float powerTarget = 3200;
    public float efficientPowerPct = 0;
    //public int unresolvedErros = 0;
    private float totalResponseTime = 0;
    private int totalEvents = 0;
    private int missedEvents = 0;
    public int operatorMistakes = 0;
    public List<LogEventData> logs = new List<LogEventData>();

    //ADD LIST OF SPECIAL EVENT LOGS AND INSTANTIATE ON UI 

    [Space(20)]
    [Header("UI")]

    [SerializeField] private TMP_Text eventsHandledTMP;
    [SerializeField] private TMP_Text avgResponseTimeTMP;
    [SerializeField] private TMP_Text powerStabilityTMP;
    [SerializeField] private TMP_Text gradeTMP;
    [SerializeField] private RectTransform logsParent;
    [SerializeField] private GameObject performanceUI;
    [SerializeField] private LogsPanel logPerformancePanel;

    private DayHandler dayHandler;

    private float AvgReponseTime()
    {
        return totalResponseTime / totalEvents;
    }
    public void AddEventTime(float time)
    {
        totalResponseTime += time;
        totalEvents++;
    }
    public void MissedEvent()
    {
        missedEvents++;
        totalEvents++;
    }
    private string CalculatePerformanceGrade()
    {
        float totalPerformance = (GradeMistakes() + GradeAverageResponseTime() + efficientPowerPct) / 3;

        switch (totalPerformance)
        {
            case > .9f:
                return "A";
            case > .8f:
                return "B";
            case > .6f:
                return "C";
            case > .4f:
                return "D";
            case > .2f:
                return "E";
            default:
                return "F";
        }
    }
    private float GradeMistakes()
    {
        switch (operatorMistakes)
        {
            case 0:
                return 1;
            case 1:
                return .8f;
            case 2:
                return .6f;
            case 3:
                return .4f;
            case 4:
                return .2f;
            case > 5:
                return 0;
            default:
                return 0;
        }
    }
    private float GradeAverageResponseTime()
    {
        switch (AvgReponseTime())
        {
            //0 means no event was launched
            case 0:
                return 0;
            case < 5:
                return 1;
            case < 7.5f:
                return .8f;
            case < 10f:
                return .6f;
            case < 12.5f:
                return .4f;
            case < 15f:
                return .2f;
            case > 15f:
                return 0;
            default:
                return 0;
        }
    }
    public void SetPerformanceUI()
    {
        float answeredEvents = totalEvents - missedEvents;
        float powerPct = efficientPowerPct * 100f;
        eventsHandledTMP.text = answeredEvents + "/" + totalEvents;
        powerStabilityTMP.text = string.Format("{0:F1}", powerPct) + "%";
        avgResponseTimeTMP.text = AvgReponseTime().ToString() + " s";
        gradeTMP.text = CalculatePerformanceGrade();
        EmitLogs();
        performanceUI.SetActive(true);
    }

    private void EmitLogs()
    {
        foreach (LogEventData logData in logs) 
        {
            logPerformancePanel.EmitPerformanceLog(logData.timeEmited, logData.logType, logData.localeLabel());
        }
    }
}
