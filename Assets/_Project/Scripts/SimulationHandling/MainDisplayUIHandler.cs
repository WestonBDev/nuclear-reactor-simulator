using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainDisplayUIHandler : MonoBehaviour
{
    [SerializeField] private MainDisplayVariablesHandler variablesHandler;
    [SerializeField] private Slider coolantTempSlider;
    [SerializeField] private TMP_Text coreTempText;
    [SerializeField] private TMP_Text voidFractionText;
    [SerializeField] private TMP_Text dateTimeText;
    [SerializeField] private TMP_Text powerText;
    [SerializeField] private Slider voidFractionSlider;
    [SerializeField] private Slider pressureSlider;
    [SerializeField] private Image xenonImage;
    [SerializeField] private Image reactivityGaugeImage;
    [SerializeField] private Image periodGaugeImage;
    [SerializeField] private float gaugeSpeed = 60f;

    public float cd = 5f;

    private float timer = 0f;
    private Vector3 gaugeAngle;

    void Start()
    {
        gaugeAngle = periodGaugeImage.transform.localEulerAngles;
    }

    void Update()
    {


        //DATE
        dateTimeText.text = DateTime.Now.ToString("HH:mm:ss");

        //COOLANT TEMP
            coolantTempSlider.value = variablesHandler.CoolantTemp();

        //CORE TEMP
            var stringValue = variablesHandler.CoreTemp();
            coreTempText.text = stringValue.ToString() + "°C";
  
        //PRESSURE
            pressureSlider.value = variablesHandler.Pressure();

        //VOID FRACTION
        voidFractionSlider.value = variablesHandler.VoidFraction();
        voidFractionText.text = Mathf.Round(variablesHandler.VoidFraction() * 100).ToString() + "%";

        //XENON
        xenonImage.transform.localScale = Vector3.one * variablesHandler.Xenon();

        //REACTIVITY
        float reactivity = Mathf.Clamp(variablesHandler.Reactivity(),.6f, 1.4f);
        float angle = Remap(reactivity, .6f, 1.4f, 1, -1f);

       // if((reactivityGaugeImage.rectTransform.localEulerAngles.z )

        if((angle < 0 && reactivityGaugeImage.rectTransform.localEulerAngles.z >= 210) || (angle > 0 && reactivityGaugeImage.rectTransform.localEulerAngles.z <= 330))
            reactivityGaugeImage.rectTransform.RotateAround(reactivityGaugeImage.rectTransform.position, Vector3.forward, angle * gaugeSpeed * Time.deltaTime);

        //POWER
        powerText.text = Mathf.Round(variablesHandler.Power()).ToString() + " MW";

        if (timer > cd)
        {
            //REACTOR_PERIOD
           // Debug.Log(variablesHandler.ReactorPeriod());
            switch (variablesHandler.ReactorPeriod())
            {
                case > 60:
                    StartCoroutine(LerpGauge(new Vector3(0, 0, 65)));
                    break;
                case < 20:
                    StartCoroutine(LerpGauge(new Vector3(0, 0, 295)));
                    break;
                default:
                    StartCoroutine(LerpGauge(new Vector3(0, 0, 350)));
                    break;
            }
            timer = 0f;
        }
        //Debug.Log(periodGaugeImage.rectTransform.localEulerAngles.z);
        timer += Time.deltaTime;
    }

    private IEnumerator LerpGauge(Vector3 targetAngle)
    {
        float lerp = 0f;
        while (lerp < cd - 1) 
        {
            yield return new WaitForSeconds(.1f);
            lerp += .1f;

            float lerpT = lerp / cd;
        Vector3 currentAngle = new Vector3(
            Mathf.LerpAngle(gaugeAngle.x, targetAngle.x, lerpT),
            Mathf.LerpAngle(gaugeAngle.y, targetAngle.y, lerpT),
            Mathf.LerpAngle(gaugeAngle.z, targetAngle.z, lerpT));

        periodGaugeImage.rectTransform.localEulerAngles = currentAngle;
        }

        gaugeAngle =
        periodGaugeImage.transform.eulerAngles;
    }

    public float Remap(float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }

}
