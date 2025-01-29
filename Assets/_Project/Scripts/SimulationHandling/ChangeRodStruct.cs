using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class ChangeRodStruct : MonoBehaviour
{
    public Slider rodOneSlider;
    public Slider rodTwoSlider;
    public Slider rodThreeSlider;
    public Slider rodFourSlider;
    public Slider rodFiveSlider;
    public ChangeSpriteState changeSpriteAutorod;

    public bool currentState = false;

    private void Start()
    {
        Invoke("ChangeRodOne", 1);
    }

    public float RodInsertionPct()
    {
        float sum = rodOneSlider.value + rodTwoSlider.value + rodThreeSlider.value + rodFourSlider.value + rodFiveSlider.value; //max 5.0
        float pct = (sum / 5f) * 100f;
        return Mathf.Abs(pct-100);
    }

    public void ChangeRodOne()
    {
        EntityManager _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityQuery colorTablesQ = _entityManager.CreateEntityQuery(new ComponentType[] { typeof(RodsPositions) });

        colorTablesQ.TryGetSingletonEntity<RodsPositions>(out Entity colorTablesEntity);


        _entityManager.SetComponentData(colorTablesEntity, new RodsPositions 
        { 
            groupOne = rodOneSlider.value,
            groupOTwo = rodTwoSlider.value,
            groupThree = rodThreeSlider.value,
            groupFour = rodFourSlider.value,
            groupFive = rodFiveSlider.value
        });
    }

    public void Scram()
    {
        DeactivateAutoRod();
        changeSpriteAutorod.Deactivate();

        EntityManager _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityQuery colorTablesQ = _entityManager.CreateEntityQuery(new ComponentType[] { typeof(RodsPositions) });

        colorTablesQ.TryGetSingletonEntity<RodsPositions>(out Entity colorTablesEntity);


        _entityManager.SetComponentData(colorTablesEntity, new RodsPositions
        {
            groupOne = 0,
            groupOTwo = 0,
            groupThree = 0,
            groupFour = 0,
            groupFive = 0
        });

        rodOneSlider.value = 0;
        rodTwoSlider.value = 0;
        rodThreeSlider.value = 0;
        rodFourSlider.value = 0;
        rodFiveSlider.value = 0;
    }

    public void ChangeAutoRodState()
    {
        EntityManager _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityQuery colorTablesQ = _entityManager.CreateEntityQuery(new ComponentType[] { typeof(RodAutoMovement) });

        colorTablesQ.TryGetSingletonEntity<RodAutoMovement>(out Entity colorTablesEntity);


        _entityManager.SetComponentData(colorTablesEntity, new RodAutoMovement
        {
            AutoRodMovement = !currentState
        });

        currentState = !currentState;
    }

    public void DeactivateAutoRod()
    {
        EntityManager _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityQuery colorTablesQ = _entityManager.CreateEntityQuery(new ComponentType[] { typeof(RodAutoMovement) });

        colorTablesQ.TryGetSingletonEntity<RodAutoMovement>(out Entity colorTablesEntity);


        _entityManager.SetComponentData(colorTablesEntity, new RodAutoMovement
        {
            AutoRodMovement = false
        });

        currentState = false;
    }

    public void SetSliders(float value)
    {
        if (!currentState) return;
        rodOneSlider.value = value;
        rodTwoSlider.value = value;
        rodThreeSlider.value = value;
        rodFourSlider.value = value;
        rodFiveSlider.value = value;
    }
}
