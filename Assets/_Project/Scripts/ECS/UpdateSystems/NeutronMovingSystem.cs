using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;

[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct NeutronMovingSystem : ISystem
{
    Unity.Mathematics.Random rand;

    [BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<Config>();

        state.RequireForUpdate<SimulationSpeed>();

        state.RequireForUpdate<ExecuteNeutronMovement>();

        state.RequireForUpdate<PauseSimulation>();

        rand = new Unity.Mathematics.Random(1000);
    }

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
    {
        PauseSimulation pauseEntity = SystemAPI.GetSingleton<PauseSimulation>();
        if (pauseEntity.Paused) return;

        state.Enabled = true;
		var config = SystemAPI.GetSingleton<Config>();
        SimulationSpeed simSpeed = SystemAPI.GetSingleton<SimulationSpeed>();
        var dt = SystemAPI.Time.DeltaTime;
		var minDist = config.UraniumScale/2f; //uranium radius
		var minDistSQ = minDist * minDist;
		var rodRadius = config.RodScale.x/2f; //0.15f in x diamaeter

		var em = state.EntityManager;

   


        foreach (var (neutronTransform, direction, data, collision, neutron) in
			SystemAPI.Query<RefRW<LocalTransform>, RefRW<Direction>, RefRW<NeutronData>, RefRW<CollisionState>>().WithEntityAccess()
			.WithAll<Neutron>())
		{

            //Limits of the "cage"
            if (neutronTransform.ValueRW.Position.y > 2.85f || neutronTransform.ValueRW.Position.y < -1.8f ||
                neutronTransform.ValueRW.Position.x > 2.2f || neutronTransform.ValueRW.Position.x < -6.8f)
            {
                em.SetComponentEnabled<CollisionState>(neutron, false);
            }

            //Heat water that comes in contact with it
            foreach (var (waterTransform, waterData)in
                  SystemAPI.Query<RefRW<LocalTransform> , RefRW<WaterData>>()
                  .WithAll<Water>())
            {
                if (math.distancesq(neutronTransform.ValueRW.Position.xy, waterTransform.ValueRO.Position.xy) <= minDistSQ)
                {
                    waterData.ValueRW.Heat += 1f * simSpeed.Multiplier;

                    //Small chance of water destorying neutron
                    var destroyChance = rand.NextFloat(0f, 1f);
                    if (destroyChance <= 0.001f / simSpeed.Multiplier)
                    {
                        em.SetComponentEnabled<CollisionState>(neutron, false);
                    }
                }
            }

            collision.ValueRW.Cooldown += dt;

            //Small cooldown on collison
            if (collision.ValueRW.Cooldown > .25f / simSpeed.Multiplier)
            {
                if (data.ValueRO.State == 0)
                {
                    foreach (var rodTransform in
                    SystemAPI.Query<RefRW<LocalTransform>>()
                    .WithAll<Rod>())
                    {
                        if (math.abs(neutronTransform.ValueRW.Position.x - rodTransform.ValueRO.Position.x) <= rodRadius &&
                            math.abs(neutronTransform.ValueRW.Position.y - rodTransform.ValueRO.Position.y) <= config.RodScale.y/2f /*half of rods length*/)
                        {
                            //Destroy tag
                            em.SetComponentEnabled<CollisionState>(neutron, false);
                            break;
                        }
                    }

                    foreach (var (uraniumTransform, uraniumState) in
                    SystemAPI.Query<RefRW<LocalTransform>, RefRW<UraniumData>>()
                    .WithAll<Uranium>())
                    {
                        if (math.distancesq(neutronTransform.ValueRW.Position.xy, uraniumTransform.ValueRO.Position.xy) <= minDistSQ)
                        {
                            // Xenon -> non reactive and destroys neutron
                            if (uraniumState.ValueRO.State == 2)
                            {
                                em.SetComponentEnabled<CollisionState>(neutron, false);
                                uraniumState.ValueRW.State = 0;
                            }

                            // Spawns new neutrons - Reactive -> small pct to turn into xenon, else goes to non reactive
                            if (uraniumState.ValueRO.State == 1)
                            {
                                //Change uranium state
                                var pct = rand.NextFloat(0f, 1f);

                                if (pct >= 0.5f)
                                {
                                    uraniumState.ValueRW.State = 1;
                                }
                                else
                                {
                                    uraniumState.ValueRW.State = 2;
                                }


                                for (int i = 0; i < 3; i++)
                                {
                                    Entity instNeutron = state.EntityManager.Instantiate(config.NeutronPrefab);

                                    state.EntityManager.SetComponentData(instNeutron, new LocalTransform
                                    {
                                        Position = new float3
                                        {
                                            x = uraniumTransform.ValueRO.Position.x,
                                            y = uraniumTransform.ValueRO.Position.y,
                                            z = -3f
                                        },
                                        Scale = .1f,
                                        Rotation = quaternion.identity
                                    });
                                    state.EntityManager.SetComponentData(instNeutron, new Direction
                                    {
                                        Value = rand.NextFloat2Direction()
                                    });
                                    state.EntityManager.SetComponentData(instNeutron, new NeutronData
                                    {
                                        State = 1
                                    });
                                }
                                em.SetComponentEnabled<CollisionState>(neutron, false);
                            }
                        }
                    }
                }

                if (data.ValueRO.State == 1)
                {
                    foreach (var moderatorTransform in
                    SystemAPI.Query<RefRW<LocalTransform>>()
                    .WithAll<Moderator>())
                    {
                        if (math.abs(neutronTransform.ValueRW.Position.x - moderatorTransform.ValueRO.Position.x) <= rodRadius)
                        {
                            //Deflect direction and change state
                            data.ValueRW.State = 0;

                            var neutronToModeratorVector = math.normalize((neutronTransform.ValueRO.Position - moderatorTransform.ValueRO.Position).xy);
                            direction.ValueRW.Value = -math.reflect(direction.ValueRO.Value, neutronToModeratorVector);
                            collision.ValueRW.Cooldown = 0;
                            break;
                        }
                    }
                }
            }





            var newPos = data.ValueRO.State == 1? neutronTransform.ValueRW.Position +
			new float3(direction.ValueRO.Value.x, direction.ValueRO.Value.y, 0) * dt * config.NeutronSpeed * 2f * simSpeed.Multiplier:
            neutronTransform.ValueRW.Position +
            new float3(direction.ValueRO.Value.x, direction.ValueRO.Value.y, 0) * dt * config.NeutronSpeed * simSpeed.Multiplier;

            neutronTransform.ValueRW.Position = newPos;
		}
	
	}
}
