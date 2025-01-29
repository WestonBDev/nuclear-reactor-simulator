using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using static UnityEngine.Rendering.DebugUI.Table;
using static UnityEngine.Rendering.STP;


[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[RequireMatchingQueriesForUpdate]
public partial struct NeutronDeletionSystem : ISystem
{
	private EntityQuery _query;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<Config>();
        state.RequireForUpdate<PauseSimulation>();

        state.RequireForUpdate<ExecuteNeutronDeletion>();

		_query = new EntityQueryBuilder(Allocator.TempJob).WithAll<Neutron>().WithDisabled<CollisionState>().Build(ref state);

	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{

        PauseSimulation pauseEntity = SystemAPI.GetSingleton<PauseSimulation>();
        if (pauseEntity.Paused) return;
        //var ecb = new EntityCommandBuffer(Allocator.TempJob);

        //ecb.DestroyEntity(_query, EntityQueryCaptureMode.AtPlayback);

        var entityArray = _query.ToEntityArray(Allocator.Temp);

		state.EntityManager.DestroyEntity(entityArray);
  //      var em = state.EntityManager;

  //      // DELETE is as expensive and leaves them rendering on screen for some reason
  //      foreach (var (collision, neutron) in
		//	SystemAPI.Query<RefRW<CollisionState>>().WithDisabled<CollisionState>().WithEntityAccess()
		//	.WithAll<Neutron>())
		//{
		//	state.EntityManager.SetComponentData(neutron, new LocalTransform
		//	{
		//		Position = new float3
		//		{
		//			x = 100000000f,
		//			y = 0f,
		//			z = 0f
		//		},
		//		Scale = 0f,
		//		Rotation = quaternion.identity
		//	});
		//}


    }
}
