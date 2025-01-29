using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

partial struct XenonCounterSystem : ISystem
{

    EntityQuery query;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ExistingXenon>();

        state.RequireForUpdate<ExecuteXenonCounter>();

        state.RequireForUpdate<PauseSimulation>();

        query = new EntityQueryBuilder(Allocator.TempJob).WithAll<Config>().Build(ref state);
        //query = state.EntityManager.CreateEntityQuery(new ComponentType[] { typeof(ExistingXenon) });
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        PauseSimulation pauseEntity = SystemAPI.GetSingleton<PauseSimulation>();
        if (pauseEntity.Paused) return;

        int counter = 0;
        foreach (var uraniumData in
                SystemAPI.Query<RefRW<UraniumData>>()
                .WithAll<Uranium>())
        {
            if(uraniumData.ValueRO.State == 2)
            {
                counter++;
            }
        }

        query.TryGetSingletonEntity<ExistingXenon>(out Entity colorTablesEntity);


        state.EntityManager.SetComponentData(colorTablesEntity, new ExistingXenon { Ammount = counter });

    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
}
