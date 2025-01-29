using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;

partial struct WaterCoolingSystem : ISystem
{
    EntityQuery query;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Config>();

        state.RequireForUpdate<SimulationSpeed>();

        state.RequireForUpdate<ExecuteWaterCooling>();

        state.RequireForUpdate<PauseSimulation>();

        state.RequireForUpdate<WaterRunning>();

        query = new EntityQueryBuilder(Allocator.TempJob).WithAll<Config>().Build(ref state);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        PauseSimulation pauseEntity = SystemAPI.GetSingleton<PauseSimulation>();
        if (pauseEntity.Paused) return;

        var dt = SystemAPI.Time.DeltaTime;

        var config = SystemAPI.GetSingleton<Config>();
        var waterRunning = SystemAPI.GetSingleton<WaterRunning>();

        SimulationSpeed simSpeed = SystemAPI.GetSingleton<SimulationSpeed>();

        float existingWater = 0;

        foreach (var water in
            SystemAPI.Query<RefRW<WaterData>>()
            .WithAll<Water>())
        {
            if (water.ValueRW.Heat < 100) existingWater++;

            water.ValueRW.Cooldown += dt;

            if(water.ValueRW.Cooldown > 0.1f / simSpeed.Multiplier)
            {
                var newHeat = water.ValueRW.Heat - 1;
                water.ValueRW.Heat = math.clamp(newHeat, 0, 150);
                water.ValueRW.Cooldown = 0;
            }
        }

        query.TryGetSingletonEntity<WaterRunning>(out Entity colorTablesEntity);


        state.EntityManager.SetComponentData(colorTablesEntity, new WaterRunning { Pct = (existingWater / config.GridSize) * 100});
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
