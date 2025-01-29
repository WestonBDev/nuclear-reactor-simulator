using System.Diagnostics;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct UraniumActivationSystem : ISystem
{
    float timer;
    bool foundValid;
    Unity.Mathematics.Random rand;
    int batchAmmount;
    int ammountFound;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Config>();

        state.RequireForUpdate<SimulationSpeed>();

        state.RequireForUpdate<ExecuteUraniumActivator>();

        state.RequireForUpdate<PauseSimulation>();

        state.RequireForUpdate<UraniumActivationAmmount>();

        rand = new Unity.Mathematics.Random(1000);
        batchAmmount = 1;
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        PauseSimulation pauseEntity = SystemAPI.GetSingleton<PauseSimulation>();
        if (pauseEntity.Paused) return;

        state.Enabled = true;

        timer += SystemAPI.Time.DeltaTime;
        foundValid = false;

        SimulationSpeed simSpeed = SystemAPI.GetSingleton<SimulationSpeed>();

        Config config = SystemAPI.GetSingleton<Config>();
        UraniumActivationAmmount activationAmmount = SystemAPI.GetSingleton<UraniumActivationAmmount>();
        batchAmmount = activationAmmount.BatchAmmount;

        if (timer > config.UraniumActivationCooldown / simSpeed.Multiplier)
        {

            while (ammountFound < batchAmmount) 
            {
                int randomIndex = rand.NextInt(0, config.Rows * config.Columns);

                foreach (var uraniumData in
                SystemAPI.Query<RefRW<UraniumData>>()
                .WithAll<Uranium>())
                {
                    if(uraniumData.ValueRO.Index == randomIndex)
                    {
                        if (uraniumData.ValueRO.State == 0)
                        {
                            uraniumData.ValueRW.State = 1;  
                        }
                        foundValid = true;
                        ammountFound++;
                        break;
                    }
                }
            }
            timer = 0f;
            ammountFound = 0;
        }
    }
}
