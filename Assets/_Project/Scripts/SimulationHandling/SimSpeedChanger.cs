using Unity.Entities;
using UnityEngine;

public class SimSpeedChanger : MonoBehaviour
{
    private float[] speeds = { 0.5f, 1, 2, 4 };
    private int speedIndex = 1;

    [SerializeField] private float currentSpeed;
    [SerializeField] private bool paused = false;

    private void Start()
    {
        currentSpeed = 1;
        ResumeSimulation();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            speedIndex++;

            if(speedIndex >= speeds.Length) { speedIndex = 0; }

            currentSpeed = speeds[speedIndex];
            Time.timeScale = currentSpeed;

            EntityManager _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityQuery simulationSpeedQ = _entityManager.CreateEntityQuery(new ComponentType[] { typeof(SimulationSpeed) });

            simulationSpeedQ.TryGetSingletonEntity<SimulationSpeed>(out Entity simulationSpeedEntity);

            _entityManager.SetComponentData(simulationSpeedEntity, new SimulationSpeed
            {
                Multiplier = speeds[speedIndex]
            });
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            paused = !paused;
            if (paused)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = currentSpeed;
            }

            EntityManager _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityQuery pauseSimQ = _entityManager.CreateEntityQuery(new ComponentType[] { typeof(PauseSimulation) });

            pauseSimQ.TryGetSingletonEntity<PauseSimulation>(out Entity pauseEntity);

            _entityManager.SetComponentData(pauseEntity, new PauseSimulation
            {
                Paused = paused                
            });
        }
    }

    public void PauseSimulation()
    {
        Time.timeScale = 0;

        EntityManager _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityQuery pauseSimQ = _entityManager.CreateEntityQuery(new ComponentType[] { typeof(PauseSimulation) });

        pauseSimQ.TryGetSingletonEntity<PauseSimulation>(out Entity pauseEntity);

        _entityManager.SetComponentData(pauseEntity, new PauseSimulation
        {
            Paused = true
        });
    }

    public void ResumeSimulation()
    {
        EntityManager _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityQuery pauseSimQ = _entityManager.CreateEntityQuery(new ComponentType[] { typeof(PauseSimulation) });

        pauseSimQ.TryGetSingletonEntity<PauseSimulation>(out Entity pauseEntity);

        Time.timeScale = currentSpeed;


        _entityManager.SetComponentData(pauseEntity, new PauseSimulation
        {
            Paused = false
        });
    }
}
