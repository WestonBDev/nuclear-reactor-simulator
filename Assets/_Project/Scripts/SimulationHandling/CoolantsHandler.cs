using System;
using UnityEngine;
using UnityEngine.UI;

public class CoolantsHandler : MonoBehaviour
{
    public bool valveOneState; //flase closed, true opened
    public bool valveTwoState;
    public float pumpSpeed; //0 to 100 according to slider

    //UI stuff
    public Toggle valveOneToggle, valveTwoToggle;
    public Sprite openValveSprite, closedValveSprite;
    
    public float CoolingPower()
    {
        var valveOnePower = valveOneState ? 1 : 0;
        var valveTwoPower = valveTwoState ? 1 : 0;

        var power = pumpSpeed * ((valveOnePower + valveTwoPower)/2f);
        //Slider at 100% with both valves closed = 0%, one open = 50%, both = 100%

        return power;
    }


    public void ChangeValveOneState(bool state)
    {
        valveOneState = state;

        if (state)
        {
            valveOneToggle.image.sprite = openValveSprite;
        }
        else
        {
            valveOneToggle.image.sprite = closedValveSprite;
        }
    }

    public void ChangeValveTwoState(bool state)
    {
        valveTwoState = state;

        if (state)
        {
            valveTwoToggle.image.sprite = openValveSprite;
        }
        else
        {
            valveTwoToggle.image.sprite = closedValveSprite;
        }
    }

    public void SetPumpSpeed(Single pumpSpeed)
    {
        this.pumpSpeed = (float)pumpSpeed;
    }
}
