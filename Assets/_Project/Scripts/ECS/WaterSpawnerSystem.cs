using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct WaterSpawnerSystem : ISystem
{

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<Config>();

		state.RequireForUpdate<ExecuteWaterSpawner>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		state.Enabled = false;


		Config config = SystemAPI.GetSingleton<Config>();

		for (int row = 0; row < config.Rows; row++)
		{
			for (int column = 0; column < config.Columns; column++)
			{
				Entity water = state.EntityManager.Instantiate(config.WaterPrefab);

				state.EntityManager.SetComponentData(water, new LocalTransform
				{
					Position = new float3
					{
						x = (column * config.UraniumSpacing) + config.GridOrigin.x,
						y = (row * config.UraniumSpacing) + config.GridOrigin.y,
						z = 0f
					},
					Scale = config.WaterScale,
					Rotation = quaternion.identity
				});
			}
		}
	}
}