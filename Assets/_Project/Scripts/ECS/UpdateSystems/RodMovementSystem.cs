using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateBefore(typeof(TransformSystemGroup))]
partial struct RodMovementSystem : ISystem
{
    EntityQuery query;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Config>();

        state.RequireForUpdate<SimulationSpeed>();

        state.RequireForUpdate<ExecuteNeutronMovement>();

        state.RequireForUpdate<PauseSimulation>();

        state.RequireForUpdate<RodsPositions>();
        state.RequireForUpdate<RodAutoMovement>();
        state.RequireForUpdate<ExistingNeutrons>();
        state.RequireForUpdate<RodInsertionPct>();

        query = new EntityQueryBuilder(Allocator.TempJob).WithAll<Config>().Build(ref state);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        PauseSimulation pauseEntity = SystemAPI.GetSingleton<PauseSimulation>();
        if (pauseEntity.Paused) return;

        var config = SystemAPI.GetSingleton<Config>();
        var rodOne = SystemAPI.GetSingleton<RodsPositions>();
        var rodAuto = SystemAPI.GetSingleton<RodAutoMovement>();
        var neutrons = SystemAPI.GetSingleton<ExistingNeutrons>();
        var insertionPct = SystemAPI.GetSingleton<RodInsertionPct>();
        SimulationSpeed simSpeed = SystemAPI.GetSingleton<SimulationSpeed>();


        var vertical = Input.GetAxis("Vertical");
        //var input = new float3(0, vertical, 0f) * SystemAPI.Time.DeltaTime /*speeed*/;
        int groupCounter = 0;

        float newPosY = 0f;

        float speed = .5f * simSpeed.Multiplier;

        if (!rodAuto.AutoRodMovement)
        {
            foreach (var rodTransform in
          SystemAPI.Query<RefRW<LocalTransform>>()
          .WithAll<Rod>())
            {
                switch (groupCounter)
                {
                    case < 2:
                        newPosY = Mathf.MoveTowards(rodTransform.ValueRO.Position.y, .55f /*base pos y*/ + (rodOne.groupOne * 4.7f), speed * SystemAPI.Time.DeltaTime);
                        break;
                    case < 4:
                        newPosY = Mathf.MoveTowards(rodTransform.ValueRO.Position.y, .55f /*base pos y*/ + (rodOne.groupOTwo * 4.7f), speed * SystemAPI.Time.DeltaTime);
                        break;
                    case < 6:
                        newPosY = Mathf.MoveTowards(rodTransform.ValueRO.Position.y, .55f /*base pos y*/ + (rodOne.groupThree * 4.7f), speed * SystemAPI.Time.DeltaTime);
                        break;
                    case < 8:
                        newPosY = Mathf.MoveTowards(rodTransform.ValueRO.Position.y, .55f /*base pos y*/ + (rodOne.groupFour * 4.7f), speed * SystemAPI.Time.DeltaTime);
                        break;
                    case < 10:
                        newPosY = Mathf.MoveTowards(rodTransform.ValueRO.Position.y, .55f /*base pos y*/ + (rodOne.groupFive * 4.7f), speed * SystemAPI.Time.DeltaTime);
                        break;
                    default:
                        break;
                }

                //var newPos = rodTransform.ValueRO.Position + input;s

                //if (input.Equals(float3.zero))
                //{
                //    return;
                //}


                rodTransform.ValueRW.Position = new float3(rodTransform.ValueRO.Position.x, newPosY, rodTransform.ValueRO.Position.z);
                groupCounter += 1;
            }
        }
        else
        {
            var input = new float3(0f, 1f, 0f);
            if (neutrons.Ammount > 40)
            {
                //Insert rods
                input = new float3(0f, -1f, 0f) * SystemAPI.Time.DeltaTime * speed;
            }
            else
            {
                input = new float3(0f, 1f, 0f) * SystemAPI.Time.DeltaTime * speed;
                //Pull rods
            }

            foreach (var rodTransform in
              SystemAPI.Query<RefRW<LocalTransform>>()
              .WithAll<Rod>())
            {
                var newPos = rodTransform.ValueRO.Position + input;
                newPos.y = math.clamp(newPos.y, .55f, 5.25f);
                rodTransform.ValueRW.Position = newPos;
            }  
        }

        float yPosSum = 0f;
        foreach (var rodTransform in
        SystemAPI.Query<RefRW<LocalTransform>>()
        .WithAll<Rod>())
        {
            yPosSum += rodTransform.ValueRO.Position.y - .55f;
        }
        float maxPos = (5.25f - .55f) * config.Rods; //0 pct insertion
        float minPos = .55f * config.Rods; //100 pct insertion 

        float pct = 1 - (yPosSum / maxPos);

        query.TryGetSingletonEntity<RodInsertionPct>(out Entity colorTablesEntity);
        state.EntityManager.SetComponentData(colorTablesEntity, new RodInsertionPct { Pct = pct});
    }
}

