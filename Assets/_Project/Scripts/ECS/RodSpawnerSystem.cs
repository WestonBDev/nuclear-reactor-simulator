using System.Linq.Expressions;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct RodSpawnerSystem : ISystem
{

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<Config>();

		state.RequireForUpdate<ExecuteRodSpawner>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		state.Enabled = false;


		Config config = SystemAPI.GetSingleton<Config>();

		var desiredNonUniformScale = float4x4.Scale(0.15f, 8.4f, 1f);

		for (int rods = 0; rods < config.Rods; rods++)
		{
			Entity rod = state.EntityManager.Instantiate(config.RodPrefab);

			state.EntityManager.SetComponentData(rod, new LocalTransform
			{
				Position = new float3
				{
					x = (rods * config.RodSpacing * 8) + (config.RodSpacing * 4) + config.GridOrigin.x - (config.WaterScale / 2f),
					y = 5.25f /*+ config.RodScale.y, hardcoded, if changed grid size has to change*/,
					z = -2f
				},
				Scale = 1f,
				Rotation = quaternion.identity
			});
			state.EntityManager.AddComponent<PostTransformMatrix>(rod);
			state.EntityManager.SetComponentData(rod, new PostTransformMatrix
			{
				Value = float4x4.Scale(config.RodScale)
			});
        }
    }
}