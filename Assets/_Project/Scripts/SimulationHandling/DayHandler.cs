using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.Entities;
using UnityEngine.UI;

public class DayHandler : MonoBehaviour
{
    [SerializeField] private float dayDurationSeconds = 60f;
    [SerializeField] private FloatVariable dayTime;
    [SerializeField] private float infoLogCd = 5f;
    [SerializeField] private List<float> eventTimes = new List<float>();
    [SerializeField] private List<LogEventData> scriptedEvents= new List<LogEventData>();
    [SerializeField] private List<LogEventData> infoEvents = new List<LogEventData>();
    [SerializeField] PerformanceTracker performanceTracker;
    [SerializeField] Button scramButton;
    [SerializeField] float powerEfficiencyGraceTime = 10f;
    [SerializeField] float powerEfficiencyGap = 1000f;

    [SerializeField] private LogsPanel logPanel;
    private ChangeRodStruct changeRodStruct;
    private MainDisplayVariablesHandler simVariables;
    private float t = 0f;
    private float eventTime = 0f;
    private int eventIndex = 0;
    private bool trackingResponseTime = false;
    private bool isOver = false;
    private float efficientPowerTime = 0f;

    private void Start()
    {
        changeRodStruct = FindAnyObjectByType<ChangeRodStruct>();
        simVariables = FindAnyObjectByType<MainDisplayVariablesHandler>();

        StartCoroutine(EmitInfoLog());
    }

    // Update is called once per frame
    void Update()
    {
        if (isOver) return;


        t += Time.deltaTime;
        dayTime.value = t;
        TrackEfficientPower();

        if (t > dayDurationSeconds)
        {
            isOver = true;
            SetEfficientPowerTime();
            performanceTracker.SetPerformanceUI();
            FindAnyObjectByType<SimSpeedChanger>().PauseSimulation();
        }

        if (eventIndex >= eventTimes.Count) return;

        if (eventTimes[eventIndex] < t)
        {
            LogEventData logData = scriptedEvents[eventIndex];
            eventIndex++;
            logPanel.EmitLog(logData.logType, logData.localeLabel(), logData);
            EmitEvent(logData.eventType);
            performanceTracker.logs.Add(logData);
        }
    }

    #region LOGS_EVENTS
    private void EmitEvent(LogEventType eventType)
    {
        switch (eventType)
        {
            case LogEventType.NONE:
                break;
            case LogEventType.TYPE1:
                AwaitResponse(scramButton);
                StartCoroutine(ReactivityIncrease());
                break;
            case LogEventType.TYPE2:
                break;
            case LogEventType.TYPE3:
                break;
            default:
                break;
        }
    }

    private void EmitInfo(LogEventData eventData)
    {
        LogEventType eventType = eventData.eventType;   
        switch (eventType)
        {
            case LogEventType.NONE:
                break;
            case LogEventType.TYPE1:
                StartCoroutine(ReactivityIncrease());
                break;
            case LogEventType.TYPE2:
                logPanel.EmitLog(eventData.logType, eventData.localeLabel() + " " + changeRodStruct.RodInsertionPct(), eventData);
                break;
            case LogEventType.TYPE3:
                logPanel.EmitLog(eventData.logType, eventData.localeLabel(), eventData);
                break;
            default:
                break;
        }
    }

    private IEnumerator EmitInfoLog()
    {
        while (true)
        {
            yield return new WaitForSeconds(infoLogCd);
            int logIndex = Random.Range(0, infoEvents.Count);
            EmitInfo(infoEvents[logIndex]);
        }
    }

    private IEnumerator ReactivityIncrease()
    {
        yield return new WaitForSeconds(2f);

        EntityManager _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityQuery neutronRateQ = _entityManager.CreateEntityQuery(new ComponentType[] { typeof(NeutronSpawnRate) });

        neutronRateQ.TryGetSingletonEntity<NeutronSpawnRate>(out Entity neutronCdEntity);


        _entityManager.SetComponentData(neutronCdEntity, new NeutronSpawnRate
        {
            cd = 0.05f
        });

        yield return new WaitForSeconds(20f);

        _entityManager.SetComponentData(neutronCdEntity, new NeutronSpawnRate
        {
            cd = 0.1f
        });

        logPanel.EmitLog(LogType.EVENT, "Reactivity normalized.");
        WasEventResponded(scramButton);
    }
    #endregion

    #region PERFORMANCE_TRACKING

    private void AwaitResponse(Button btn)
    {
        trackingResponseTime = true;
        StartCoroutine(TrackResponseTimeCor());
        btn.onClick.AddListener(() => ResponseGiven(btn));
    }

    private void ResponseGiven(Button btn)
    {
        trackingResponseTime = false;
        btn.onClick.RemoveAllListeners();
        performanceTracker.AddEventTime(eventTime);
    }

    private void WasEventResponded(Button btn)
    {
        //event wasnt answered, add to operator mistakes
        if (trackingResponseTime)
        {
            btn.onClick.RemoveAllListeners();
            performanceTracker.operatorMistakes++;
            trackingResponseTime = false;
            performanceTracker.MissedEvent();
        }
    }

    private IEnumerator TrackResponseTimeCor()
    {
        eventTime = 0;
        while (trackingResponseTime)
        {
            yield return new WaitForSeconds(0.1f);
            eventTime += .1f;
        }
    }

    private void TrackEfficientPower()
    {
        //Between admitted threshold add time, else don't
        if(simVariables.Power() > (performanceTracker.powerTarget - powerEfficiencyGap) &&
            simVariables.Power() < (performanceTracker.powerTarget + powerEfficiencyGap))
        {
            efficientPowerTime += Time.deltaTime;
        }
    }

    public void SetEfficientPowerTime()
    {
        performanceTracker.efficientPowerPct = efficientPowerTime / dayDurationSeconds;
    }

    #endregion
}
