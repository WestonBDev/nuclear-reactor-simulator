using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace Unity.Rendering
{
    [MaterialProperty("_Color")]
    public struct MyOwnColor : IComponentData
    {
        public float4 Value;
    }
}
