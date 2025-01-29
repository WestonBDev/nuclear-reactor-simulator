using Unity.Entities;
using UnityEngine;

public class ExecuteAuthoring : MonoBehaviour
{
    public bool UraniumSpawner;
    public bool NeutronSpawner;
    public bool NeutronMovement;
	public bool NeutronDeletion;
	public bool WaterSpawner;
	public bool RodSpawner;
    public bool UraniumActivator;
    public bool ModeratorSpawner;
    public bool WaterCooling;
    public bool XenonCounter;

    class Baker : Baker<ExecuteAuthoring>
    {
        public override void Bake (ExecuteAuthoring authoring)
        {
            var entity = GetEntity (TransformUsageFlags.None);

            if (authoring.UraniumSpawner) AddComponent<ExecuteUraniumSpawner>(entity);
			if (authoring.NeutronSpawner) AddComponent<ExecuteNeutronSpawner>(entity);
			if (authoring.NeutronMovement) AddComponent<ExecuteNeutronMovement>(entity);
			if (authoring.NeutronDeletion) AddComponent<ExecuteNeutronDeletion>(entity);
			if (authoring.WaterSpawner) AddComponent<ExecuteWaterSpawner>(entity);
			if (authoring.RodSpawner) AddComponent<ExecuteRodSpawner>(entity);
            if (authoring.UraniumActivator) AddComponent<ExecuteUraniumActivator>(entity);
            if(authoring.ModeratorSpawner) AddComponent<ExecuteModeratorSpawner>(entity);
            if (authoring.WaterCooling) AddComponent<ExecuteWaterCooling>(entity);
            if (authoring.XenonCounter) AddComponent<ExecuteXenonCounter>(entity);
        }
    }
}

public struct ExecuteNeutronMovement : IComponentData { }
public struct ExecuteNeutronSpawner : IComponentData { }
public struct ExecuteUraniumSpawner : IComponentData { }
public struct ExecuteNeutronDeletion : IComponentData { }
public struct ExecuteWaterSpawner : IComponentData { }
public struct ExecuteRodSpawner : IComponentData { }
public struct ExecuteUraniumActivator : IComponentData { }
public struct ExecuteModeratorSpawner : IComponentData { }
public struct ExecuteWaterCooling : IComponentData { }
public struct ExecuteXenonCounter : IComponentData { }
