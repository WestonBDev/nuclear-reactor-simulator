using System.Collections.Generic;
using Unity.Entities;
using Unity.Entities.Serialization;
using Unity.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneChanger : MonoBehaviour
{




    public void ChangeScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadGameScene()
    {
        SubsceneLoader.Instance.StartGameAtLevel();
    }

    //public void ResetSim()
    //{
    //    var entitymanager = World.DefaultGameObjectInjectionWorld.EntityManager;
    //    entitymanager.DestroyEntity(entitymanager.UniversalQuery);

    //    var defaultWorld = World.DefaultGameObjectInjectionWorld;
    //    defaultWorld.EntityManager.CompleteAllTrackedJobs();
    //    foreach (var system in defaultWorld.Systems)
    //    {
    //        system.Enabled = false;
    //    }
    //    defaultWorld.Dispose();
    //    DefaultWorldInitialization.Initialize("Default World", false);
    //    if (!ScriptBehaviourUpdateOrder.IsWorldInCurrentPlayerLoop(World.DefaultGameObjectInjectionWorld))
    //    {
    //        ScriptBehaviourUpdateOrder.AppendWorldToCurrentPlayerLoop(World.DefaultGameObjectInjectionWorld);
    //    }
    //    SceneManager.LoadScene("Reactor", LoadSceneMode.Single);
    //}


}
