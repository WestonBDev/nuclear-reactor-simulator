using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

partial struct NeutronCounterSystem : ISystem
{
    EntityQuery query;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ExistingNeutrons>();
        state.RequireForUpdate<PauseSimulation>();

        query = new EntityQueryBuilder(Allocator.TempJob).WithAll<Config>().Build(ref state);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        PauseSimulation pauseEntity = SystemAPI.GetSingleton<PauseSimulation>();
        if (pauseEntity.Paused) return;

        int counter = 0;
        foreach (var uraniumData in
                SystemAPI.Query<RefRW<LocalTransform>>()
                .WithAll<Neutron>())
        {
            counter++;
        }


        query.TryGetSingletonEntity<ExistingNeutrons>(out Entity colorTablesEntity);


        state.EntityManager.SetComponentData(colorTablesEntity, new ExistingNeutrons { Ammount = counter });

    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
