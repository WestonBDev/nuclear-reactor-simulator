using Unity.Entities;
using UnityEngine;

public class NeutronsVarHandler : MonoBehaviour
{

    public float timer = 0f;
    public float cd = .5f;
    public FloatVariable neutrons;
    public FloatVariable xenon;
    public FloatVariable voidFractionMult;
    public FloatVariable waterPct;
    public FloatVariable rodInsertionPct;

    private MainDisplayVariablesHandler mainVars;

    private void Awake()
    {
        mainVars = FindFirstObjectByType<MainDisplayVariablesHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > cd)
        {
            EntityManager _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            //neutrons
            EntityQuery colorTablesQ = _entityManager.CreateEntityQuery(new ComponentType[] { typeof(ExistingNeutrons) });
            colorTablesQ.TryGetSingletonEntity<ExistingNeutrons>(out Entity colorTablesEntity);
            neutrons.value = _entityManager.GetComponentData<ExistingNeutrons>(colorTablesEntity).Ammount;

            //xenon
            EntityQuery xenonQ = _entityManager.CreateEntityQuery(new ComponentType[] { typeof(ExistingXenon) });
            xenonQ.TryGetSingletonEntity<ExistingXenon>(out Entity xenonEntity);
            xenon.value = _entityManager.GetComponentData<ExistingXenon>(xenonEntity).Ammount;

            //void fraction
            voidFractionMult.value = mainVars.VoidFractionMultiplier();
            EntityQuery voidFractionQ = _entityManager.CreateEntityQuery(new ComponentType[] { typeof(VoidFraction) });
            voidFractionQ.TryGetSingletonEntity<VoidFraction>(out Entity voidFractionEntity);
            _entityManager.SetComponentData(voidFractionEntity, new VoidFraction
            {
                Multiplier = voidFractionMult.value
            });

            //water
            EntityQuery waterQ = _entityManager.CreateEntityQuery(new ComponentType[] { typeof(WaterRunning) });
            waterQ.TryGetSingletonEntity<WaterRunning>(out Entity waterEntity);
            waterPct.value = _entityManager.GetComponentData<WaterRunning>(waterEntity).Pct;

            EntityQuery rodQ = _entityManager.CreateEntityQuery(new ComponentType[] { typeof(RodInsertionPct) });
            rodQ.TryGetSingletonEntity<RodInsertionPct>(out Entity rodEntity);
            rodInsertionPct.value = _entityManager.GetComponentData<RodInsertionPct>(rodEntity).Pct;

            timer = 0f;
        }

        timer += Time.deltaTime;
    }
}
