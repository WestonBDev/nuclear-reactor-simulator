using System.ComponentModel.Design;
using System.Drawing;
using System.Threading;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

[BurstCompile]
[RequireMatchingQueriesForUpdate]
public partial class ColorSystem : SystemBase
{
    
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Config>();
        state.RequireForUpdate<PauseSimulation>();
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
        PauseSimulation pauseEntity = SystemAPI.GetSingleton<PauseSimulation>();
        if (pauseEntity.Paused) return;

        //var elapsedTime = SystemAPI.Time.ElapsedTime;
        //Entities.ForEach((ref MyOwnColor color, in Rod r) =>
        //{
        //	color.Value = new float4(
        //		math.cos( 1.0f),
        //		math.cos(2.0f),
        //		math.cos( 3.0f),
        //		1.0f);
        //})
        //   .ScheduleParallel();


        //foreach (var (color, rod) in
        //	SystemAPI.Query<RefRW<MaterialColor>>().WithEntityAccess().WithAll<Rod>())
        //{
        //	state.EntityManager.SetComponentData(rod, new MyOwnColor
        //	{
        //		Value = 1f
        //	});
        //}

        EntityManager entityManager = EntityManager;

        Entities
		.ForEach((ref UraniumData uranium, ref MyOwnColor materialPropertyBaseColor) =>
        {
            if (uranium.State == 0)
            {
                //Inactive
                materialPropertyBaseColor.Value = new float4(0.5f, 0.5f, 0.5f, 1);
            }
            else if (uranium.State == 1) 
            {
                //Reactive
                materialPropertyBaseColor.Value = new float4(0.13f, 0.54f, 0.98f, 1);
            }
            else
            {
                //Xenon
                materialPropertyBaseColor.Value = new float4(0.16f, 0.16f, 0.16f, 1);
            }          
        })
        .Schedule();

        Entities
       .ForEach((ref NeutronData neutron, ref MyOwnColor materialPropertyBaseColor) =>
       {
           if (neutron.State == 0)
           {
               //Inactive
               materialPropertyBaseColor.Value = new float4(0.16f, 0.16f, 0.16f, 1);
           }
           else if (neutron.State == 1)
           {
               //Reactive
               materialPropertyBaseColor.Value = new float4(1, 1, 1, 1);
           }
       })
       .Schedule();

       Entities
      .ForEach((ref WaterData water, ref MyOwnColor materialPropertyBaseColor) =>
      {
          float heatLvl = water.Heat / 100f; //0 to 1.5

          if(heatLvl < 1)
          {
              materialPropertyBaseColor.Value = math.lerp(new float4(0.86f, 0.92f, 0.99f, 1), new float4(0.98f, 0.36f, 0.34f, 1), heatLvl);
          }
          else
          {
              materialPropertyBaseColor.Value = new float4(1, 1, 1, 0);
          }

      })
      .Run();
    }
}