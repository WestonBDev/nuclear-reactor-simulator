using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class RodAuthoring : MonoBehaviour
{
	class Baker : Baker<RodAuthoring>
	{
		public override void Bake(RodAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			//Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<Rod>(entity);
		}
	}
}

public struct Rod : IComponentData
{
}

 