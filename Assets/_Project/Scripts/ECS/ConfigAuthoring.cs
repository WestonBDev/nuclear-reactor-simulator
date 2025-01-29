using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class ConfigAuthoring : MonoBehaviour
{
    public GameObject UraniumPrefab;
	public GameObject NeutronPrefab;
	public GameObject WaterPrefab;
	public GameObject RodPrefab;
	public GameObject ModeratorRodPrefab;
	public int Columns;
    public int Rows;
	public int Rods;
    public FloatVariable RodTestValue;
    public float NeutronScale = .1f;
    public float UraniumScale = .25f;
    public float WaterScale = .35f;
    public Vector3 RodScale /*= { 0.15f, 8.4f, 1f }*/;
	public float UraniumSpacing;
    public float RodSpacing;
    public float WhiteRodSpacing;
    public float NeutronCooldown;
    public float NeutronSpeed;
    public float UraniumActivationCooldown;
    public bool AutoRodMovement;
    //public float WaterHeatLimit;
    public Vector2 GridOrigin;
    public float SimSpeedMultiplier = 1f;



    class Baker : Baker<ConfigAuthoring>
    {
        public override void Bake(ConfigAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new Config
            {
                Columns = authoring.Columns,
				Rows = authoring.Rows,
				Rods = authoring.Rods,
                NeutronScale = authoring.NeutronScale,
                UraniumScale = authoring.UraniumScale,
                RodScale = authoring.RodScale,
                WaterScale = authoring.WaterScale,
                UraniumSpacing  = authoring.UraniumSpacing,
                GridOrigin = authoring.GridOrigin,  
				RodSpacing = authoring.RodSpacing,
                NeutronSpeed = authoring.NeutronSpeed,
                UraniumActivationCooldown = authoring.UraniumActivationCooldown,
                GridSize = authoring.Rows * authoring.Columns,
                //WaterHeatLimit = authoring.WaterHeatLimit,

                UraniumPrefab = GetEntity(authoring.UraniumPrefab, TransformUsageFlags.None),
				NeutronPrefab = GetEntity(authoring.NeutronPrefab, TransformUsageFlags.Dynamic),
				WaterPrefab = GetEntity(authoring.WaterPrefab, TransformUsageFlags.None),
				RodPrefab = GetEntity(authoring.RodPrefab, TransformUsageFlags.Dynamic),
                ModeratorPrefab = GetEntity(authoring.ModeratorRodPrefab, TransformUsageFlags.Dynamic)
            });

            AddComponent(entity, new RodsPositions
            {
                groupOne = 0f,
                groupOTwo = 0f,
                groupThree = 0f,
                groupFour = 0f,
                groupFive = 0f
            });

            AddComponent(entity, new ExistingNeutrons
            {
                Ammount = 0
            });

            AddComponent(entity, new RodAutoMovement
            {
                AutoRodMovement = authoring.AutoRodMovement
            });

            AddComponent(entity, new ExistingXenon
            {
                Ammount = 0
            });

            AddComponent(entity, new VoidFraction
            {
                Multiplier = 0
            });

            AddComponent(entity, new SimulationSpeed
            {
                Multiplier = authoring.SimSpeedMultiplier
            });

            AddComponent(entity, new WaterRunning
            {
                Pct = 100
            });

            AddComponent(entity, new RodInsertionPct
            {
                Pct = 0
            });

            AddComponent(entity, new NeutronSpawnRate
            {
                cd = authoring.NeutronCooldown
            });

            AddComponent(entity, new PauseSimulation
            {
                Paused = false
            });

            AddComponent(entity, new UraniumActivationAmmount
            {
                BatchAmmount = 1
            });
        }
    }

    public void SetNeutronCD(float cd)
    {
        NeutronCooldown = cd;
    }
}


public struct Config : IComponentData
{
    public Entity UraniumPrefab;
    public Entity NeutronPrefab;
    public Entity WaterPrefab;
    public Entity RodPrefab;
    public Entity ModeratorPrefab;
    public int Columns;
    public int Rows;
    public int Rods;
    public int GridSize;
    public float NeutronScale;
    public float UraniumScale;
    public float WaterScale;
    public float3 RodScale;
    public float UraniumSpacing;
    public float RodSpacing;
    public float NeutronSpeed;
    public float UraniumActivationCooldown;
    //public float WaterHeatLimit;
    public Vector2 GridOrigin;
}

public struct NeutronSpawnRate : IComponentData
{
    public float cd;
}

public struct RodsPositions : IComponentData
{
    public float groupOne;
    public float groupOTwo;
    public float groupThree;
    public float groupFour;
    public float groupFive;
}

public struct RodAutoMovement : IComponentData
{
    public bool AutoRodMovement;
}

public struct ExistingNeutrons : IComponentData
{
    public int Ammount;
}

public struct ExistingXenon : IComponentData
{
    public int Ammount;
}

public struct VoidFraction : IComponentData
{
    public float Multiplier;
}

public struct SimulationSpeed : IComponentData
{
    public float Multiplier;
}

public struct WaterRunning : IComponentData
{
    public float Pct;
}

public struct RodInsertionPct : IComponentData
{
    public float Pct;
}

public struct PauseSimulation : IComponentData
{
    public bool Paused;
}

public struct UraniumActivationAmmount : IComponentData
{
    public int BatchAmmount;
}

