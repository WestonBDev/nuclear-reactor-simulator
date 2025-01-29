using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct NeutronSpawnerSystem : ISystem
{

	float timer;
	Unity.Mathematics.Random rand;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<Config>();

        state.RequireForUpdate<SimulationSpeed>();

        state.RequireForUpdate<VoidFraction>();

        state.RequireForUpdate<ExecuteNeutronSpawner>();

        state.RequireForUpdate<PauseSimulation>();

        state.RequireForUpdate<VoidFraction>();

        state.RequireForUpdate<NeutronSpawnRate>();

        rand = new Unity.Mathematics.Random(1000);//
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
        PauseSimulation pauseEntity = SystemAPI.GetSingleton<PauseSimulation>();
        if (pauseEntity.Paused) return;

        timer += SystemAPI.Time.DeltaTime;
		

		Config config = SystemAPI.GetSingleton<Config>();
		NeutronSpawnRate neutronRate = SystemAPI.GetSingleton<NeutronSpawnRate>();
        VoidFraction voidFraction = SystemAPI.GetSingleton<VoidFraction>();
        SimulationSpeed simSpeed = SystemAPI.GetSingleton<SimulationSpeed>();

        if (timer > (neutronRate.cd / voidFraction.Multiplier / simSpeed.Multiplier))
		{

			int randomColumn = UnityEngine.Random.Range(0, config.Columns);
			int randomRow = UnityEngine.Random.Range(0, config.Rows);

			Entity neutron = state.EntityManager.Instantiate(config.NeutronPrefab);

			state.EntityManager.SetComponentData(neutron, new LocalTransform
			{
				Position = new float3
				{
					x = (randomColumn * config.UraniumSpacing) + config.GridOrigin.x,
					y = (randomRow * config.UraniumSpacing) + config.GridOrigin.y,
					z = -3f
				},
				Scale = config.NeutronScale,
				Rotation = quaternion.identity
			});
			state.EntityManager.SetComponentData(neutron, new Direction
			{
				Value = rand.NextFloat2Direction()
			});
			state.EntityManager.SetComponentData(neutron, new NeutronData
			{
				State = 1
			});

            timer = 0f;
		}
	}
}