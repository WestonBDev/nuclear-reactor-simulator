using Unity.Entities;
using Unity.Mathematics;

namespace Unity.Rendering
{
    [UnityEngine.DisallowMultipleComponent]
    public class MyOwnColorAuthoring : UnityEngine.MonoBehaviour
    {
        [Unity.Entities.RegisterBinding(typeof(MyOwnColor), nameof(MyOwnColor.Value))]
        public UnityEngine.Color color;

        class URPMaterialPropertyBaseColorBaker : Unity.Entities.Baker<MyOwnColorAuthoring>
        {
            public override void Bake(MyOwnColorAuthoring authoring)
            {
                Unity.Rendering.MyOwnColor component = default(Unity.Rendering.MyOwnColor);
                float4 colorValues;
                colorValues.x = authoring.color.linear.r;
                colorValues.y = authoring.color.linear.g;
                colorValues.z = authoring.color.linear.b;
                colorValues.w = authoring.color.linear.a;
                component.Value = colorValues;
                var entity = GetEntity(TransformUsageFlags.Renderable);
                AddComponent(entity, component);
            }
        }
    }
}