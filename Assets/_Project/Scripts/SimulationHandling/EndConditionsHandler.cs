using System.Collections;
using Unity.Entities;
using UnityEngine;

public class EndConditionsHandler : MonoBehaviour
{
    [SerializeField] private float tickDuration = 1f;
    [SerializeField] private MainDisplayVariablesHandler simVariables;
    [SerializeField] private FloatVariable neutrons;
    [SerializeField] private FloatVariable dayTime;


    [Header("Loss Thresholds")]
    [SerializeField] private float maxNeutrons = 100f;
    [SerializeField] private float maxCoreTemp = 900f;
    [SerializeField] private float maxPressure = 17f;
    [SerializeField] private float maxVoidFraction = .5f;
    [SerializeField] private float lossSequenceSeconds = 5f;
    [SerializeField] private int increaseUraniumBatch = 20;
    [SerializeField] private GameObject lossUI;
    
    

    /* Fail
     * - Core temp &gt;900°C
     * - Pressure &gt;17 MPa
     * - Void &gt;50%
     * > Than 100 neutrons
     */

    private float tickTimer = 0f;
    private float t = 0f;
    private DayHandler dayHandler;
    private SimSpeedChanger simSpeedChanger;

    private void Start()
    {
        dayHandler = FindAnyObjectByType<DayHandler>();
        simSpeedChanger = FindAnyObjectByType<SimSpeedChanger>();
    }

    private void Update()
    {
        tickTimer += Time.deltaTime;

        //if (t > dayTime.value) Win();

        if(tickTimer > tickDuration)
        {
            if (simVariables.CoreTemp() > maxCoreTemp || simVariables.Pressure() > maxPressure ||
                simVariables.VoidFraction() > maxVoidFraction || neutrons.value > maxNeutrons)
            {
                Loss();
            }
            tickTimer = 0f;
        }
    }

    //public void Win()
    //{
    //    if (!dayHandler) return;
        
    //    dayHandler.SetEfficientPowerTime();
    //    Debug.Log("Win condition met");
    //}

    public void Loss()
    {
        Debug.Log("Loss condition met");
        StartCoroutine(LossSequence());
        // Change ecs floats to generate crash scenario
    }

    private IEnumerator LossSequence()
    {
        EntityManager _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityQuery uraniumAmmountQ = _entityManager.CreateEntityQuery(new ComponentType[] { typeof(UraniumActivationAmmount) });

        uraniumAmmountQ.TryGetSingletonEntity<UraniumActivationAmmount>(out Entity uraniumEntity);


        _entityManager.SetComponentData(uraniumEntity, new UraniumActivationAmmount
        {
            BatchAmmount = increaseUraniumBatch
        });

        yield return new WaitForSeconds(lossSequenceSeconds);

        simSpeedChanger.PauseSimulation();
        lossUI.SetActive(true);
    }
}
