using UnityEngine;

public class PowerRateTracker : MonoBehaviour
{
    //Will track a change of X ammount of power and tell how many seconds it took, to help on the reactor period equation
    public float powerStepAmmount = 500;

    private MainDisplayVariablesHandler mainDisplayVariablesHandler;

    private float previousPower = 1000f;
    private float currentPower;
    private bool start;
    private float t;
    private float timeStep;

    public float ChangeRate()
    {
        return Mathf.Abs(previousPower - currentPower) / timeStep;
    }

    void Start()
    {
        mainDisplayVariablesHandler = FindAnyObjectByType<MainDisplayVariablesHandler>();
        Invoke("StartTracking", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!start) return;

        if (Mathf.Abs(currentPower - mainDisplayVariablesHandler.Power()) > powerStepAmmount)
        {
            previousPower = currentPower;
            currentPower = mainDisplayVariablesHandler.Power();
            timeStep = t;
            t = 0;
        }

        t += Time.deltaTime;
    }

    private void StartTracking()
    {
        currentPower = mainDisplayVariablesHandler.Power();
        start = true;
    }
}
