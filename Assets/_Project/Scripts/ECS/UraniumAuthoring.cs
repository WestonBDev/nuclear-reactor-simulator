using UnityEngine;
using Unity.Entities;

public class UraniumAuthoring : MonoBehaviour
{
    class Baker : Baker<UraniumAuthoring> 
    {
		public override void Bake(UraniumAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.None);
			AddComponent<Uranium>(entity);
            AddComponent<UraniumData>(entity);
        }
	}
}

public struct Uranium : IComponentData
{
}

public struct UraniumData : IComponentData
{
    //0 = Empty, 1 = Reactive,  2 = Xenon
    public int State;
    public int Index;
}
