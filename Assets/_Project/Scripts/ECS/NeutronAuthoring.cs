using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class NeutronAuthoring : MonoBehaviour
{
	public AudioClip AudioClip;
	class Baker : Baker<NeutronAuthoring>
	{
		public override void Bake(NeutronAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<Neutron>(entity);
			AddComponent<Direction>(entity);
            AddComponent<CollisionState>(entity);
            AddComponent<NeutronData>(entity);
            SetComponentEnabled<CollisionState>(entity, true);
		}
	}
}

public struct Neutron : IComponentData
{

}

public struct Direction : IComponentData
{
	public float2 Value;
}

public struct CollisionState : IComponentData, IEnableableComponent
{
	public float Cooldown;
	public float Heatcooldown;
}

public struct NeutronData : IComponentData
{
	//0 Fast Neutron (fast, cant hit rods nor other elementes only moderator rods)
	// - 1 Thermal neutron (can react with xenon, and uranium)
	public int State;
}

