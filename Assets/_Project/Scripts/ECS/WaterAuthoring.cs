using UnityEngine;
using Unity.Entities;

public class WaterAuthoring : MonoBehaviour
{
	class Baker : Baker<WaterAuthoring>
	{
		public override void Bake(WaterAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.None);
			AddComponent<Water>(entity);
            AddComponent<WaterData>(entity);
        }
	}
}

public struct Water : IComponentData
{
}

public struct WaterData : IComponentData
{
    public float Heat;
    public float Cooldown;
}
