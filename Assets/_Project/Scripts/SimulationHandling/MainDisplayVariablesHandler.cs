using UnityEngine;

public class MainDisplayVariablesHandler : MonoBehaviour
{
    [SerializeField] private CoolantsHandler coolantsHandler;
    [SerializeField] private FloatVariable neutrons;
    [SerializeField] private FloatVariable xenon;
    [SerializeField] private PowerRateTracker powerRateTracker;


    public float CoolantTemp()
    {
        float baseTemp = 200f;

        float coolingPower = coolantsHandler.CoolingPower() / 100f; /*0-1*/

        float coolantTemp = 200 + (CoreTemp() * (1 - coolingPower));

        return coolantTemp;
    }

    public float CoreTemp()
    {
        var neutronTemp = neutrons.value - (coolantsHandler.CoolingPower()/2); //50+ neutrons would start raising temprature
        var coreTemp = (Mathf.RoundToInt(Mathf.Clamp(neutronTemp, 0,1000)) * 5) /*reaches critical 900-1000 on 100 neutrons*/ + 500/*base temp*/;
        return coreTemp;
    }

    public float VoidFraction()
    {
        float voidFraction = (CoreTemp() / 1000f) * (1 - (Pressure() / 20)) * (1 - (coolantsHandler.CoolingPower() / 100f));
        float returnValue = voidFraction > 0 ? voidFraction : 0;
        return returnValue;
    }

    public float VoidFractionMultiplier()
    {
        return 1 + (VoidFraction() * 0.2f);
    }

    public float Xenon()
    {
        //xenon poisoning
        var xenonConcentration = xenon.value / 400/*fixed limit, a fourth of all uranium ammount?*/;
        return xenonConcentration;
    }

    public float Pressure()
    {
        if (CoreTemp() < 500) { return 0f; }

        float steamRate = (CoreTemp() - 500f) / 5f;

        float pressure = 10f + ((CoreTemp() - 500f) / 50f) - (coolantsHandler.CoolingPower() / 10f) + (steamRate / 10f);

        return pressure;
    }

    public float Power()
    {
        //80 power per neutron, so 3200 at around 40

        float power = neutrons.value * 80;
        float currentPower = power * (1 - Xenon()) * (VoidFractionMultiplier());

        return currentPower;
    }

    public float ReactorPeriod()
    {
        return Power() / powerRateTracker.ChangeRate();
    }

    public float Reactivity()
    {
        float neutronBaseLevel = 40f;

        float reactivity = (neutrons.value - neutronBaseLevel) / neutronBaseLevel;

        return 1f + reactivity; /* IN A RANGE OF .6 AND 1.4 usually 1.0 the center of the gauge*/
    }

    public float ModeratorEfficiency()
    {
        float baseEfficiency = 0.95f;
        float tempFactor = 1 - ((CoreTemp() - 500f) / 1000f);
        float densityFactor = 1 - (VoidFraction() * 0.3f);
        float poisonFactor = 1 - ((xenon.value / 100) * 0.5f);

        float finalEfficiency = baseEfficiency * tempFactor + densityFactor * poisonFactor; 

        return finalEfficiency;
    }
}
