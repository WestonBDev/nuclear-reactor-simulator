using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class DiagramPanel : MonoBehaviour
{
    [SerializeField] private Color operational, critical, warning, failure, disabled;
    [SerializeField] private Image rodOne, rodTwo, rodThree, pumpOne, pumpTwo, condenser, steamSeparator, turbine;
    private MainDisplayVariablesHandler mainDisplayVariablesHandler;
    private CoolantsHandler coolantsHandler;
    private ChangeRodStruct changeRodStruct;

    public float diagramUpdateTick = 1f;
    private float t = 0;

    void Start()
    {
        coolantsHandler = FindAnyObjectByType<CoolantsHandler>();
        mainDisplayVariablesHandler = FindAnyObjectByType<MainDisplayVariablesHandler>();
        changeRodStruct = FindAnyObjectByType<ChangeRodStruct>();   
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;

        if(t > diagramUpdateTick)
        {
            UpdateDiagramColors();
            t = 0;
        }
    }

    private void UpdateDiagramColors()
    {
        UpdatePumps();
        UpdateRods();
        UpdateSteamSeparator();
        UpdateTurbine();
        UpdateCondenser();
    }

    private void UpdatePumps()
    {
        if (coolantsHandler.valveOneState)
        {
            pumpOne.color = operational;
        }
        else
        {
            pumpOne.color = disabled;
        }

        if (coolantsHandler.valveTwoState)
        {
            pumpTwo.color = operational;
        }
        else
        {
            pumpTwo.color = disabled;
        }
    }

    private void UpdateRods()
    {
        if (changeRodStruct.currentState)
        {
            rodOne.color = operational;
            rodTwo.color = operational;
            rodThree.color = operational;
        }
        else
        {
            rodOne.color = disabled;
            rodTwo.color = disabled;
            rodThree.color = disabled;
        }
    }

    private void UpdateSteamSeparator()
    {
        switch (mainDisplayVariablesHandler.Pressure())
        {
            case < 15:
                steamSeparator.color = operational;
                break;
            case >= 15 and <= 17:
                steamSeparator.color = warning;
                break;
            case > 17 and <= 20:
                steamSeparator.color = critical;
                break;
            case > 20:
                steamSeparator.color = failure;
                break;
            default:
                break;
        }
    }

    private void UpdateCondenser()
    {
        switch (mainDisplayVariablesHandler.Power())
        {
            case < 3200:
                condenser.color = operational;
                break;
            case >= 3200 and <= 4000:
                condenser.color = warning;
                break;
            case > 4000 and <= 5000:
                condenser.color = critical;
                break;
            case > 5000:
                condenser.color = failure;
                break;
            default:
                break;
        }
    }

    private void UpdateTurbine()
    {
        switch (mainDisplayVariablesHandler.CoreTemp())
        {
            case < 700:
                turbine.color = operational;
                break;
            case >= 750 and <= 900:
                turbine.color = warning;
                break;
            case > 900 and <= 1000:
                turbine.color = critical;
                break;
            case > 1000:
                turbine.color = failure;
                break;
            default:
                break;
        }
    }
}
