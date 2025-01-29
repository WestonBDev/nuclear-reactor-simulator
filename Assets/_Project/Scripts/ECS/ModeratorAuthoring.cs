using Unity.Entities;
using UnityEngine;


public class ModeratorAuthoring : MonoBehaviour
{
    class Baker : Baker<ModeratorAuthoring>
    {
        public override void Bake(ModeratorAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            //Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<Moderator>(entity);
        }
    }
}

public struct Moderator : IComponentData
{
}
