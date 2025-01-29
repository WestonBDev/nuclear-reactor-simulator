using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct ModeratorSpawnerSystem : ISystem
{

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Config>();

        state.RequireForUpdate<ExecuteModeratorSpawner>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;


        Config config = SystemAPI.GetSingleton<Config>();

        var desiredNonUniformScale = float4x4.Scale(0.15f, 8.4f, 1f);

        for (int moderators = 0; moderators < 11; moderators++)
        {
            Entity moderator = state.EntityManager.Instantiate(config.ModeratorPrefab);

            state.EntityManager.SetComponentData(moderator, new LocalTransform
            {
                Position = new float3
                {
                    x = (moderators * config.RodSpacing * 8) + config.GridOrigin.x - (config.WaterScale / 2f),
                    y = ((config.Rows / 2) * config.UraniumSpacing) + config.GridOrigin.y,
                    z = -2f
                },
                Scale = 1f,
                Rotation = quaternion.identity
            });
            state.EntityManager.AddComponent<PostTransformMatrix>(moderator);
            state.EntityManager.SetComponentData(moderator, new PostTransformMatrix
            {
                Value = float4x4.Scale(config.RodScale)
            });
        }
    }
}
