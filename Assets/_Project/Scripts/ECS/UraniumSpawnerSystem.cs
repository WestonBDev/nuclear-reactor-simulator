using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct UraniumSpawnerSystem : ISystem
{

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<Config>();

		state.RequireForUpdate<ExecuteUraniumSpawner>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		state.Enabled = false;


		Config config = SystemAPI.GetSingleton<Config>();

		for (int row = 0;  row < config.Rows; row++)
		{
			for (int column = 0; column < config.Columns; column++)
			{
				Entity uranium = state.EntityManager.Instantiate(config.UraniumPrefab);

				state.EntityManager.SetComponentData(uranium, new LocalTransform
				{
					Position = new float3
					{
						x = (column * config.UraniumSpacing) + config.GridOrigin.x,
						y = (row * config.UraniumSpacing) + config.GridOrigin.y,
						z = -1f
					},
					Scale = config.UraniumScale,
					Rotation = quaternion.identity
				});
                state.EntityManager.SetComponentData(uranium, new UraniumData
                {
                    State = 0,
					Index = row +  (config.Rows * column)
                });
            }
		}
	}
}