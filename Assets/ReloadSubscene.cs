using NUnit.Framework.Internal;
using Unity.Entities;
using Unity.Entities.Serialization;
using Unity.Scenes;
using UnityEngine;

public class ReloadSubscene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public EntitySceneReference LevelScene;
    void Start()
    {
        LoadSubScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadSubScene()
    {
        World.DisposeAllWorlds();

        DefaultWorldInitialization.Initialize("Default World", false);

        SceneSystem.LoadSceneAsync(
        World.DefaultGameObjectInjectionWorld.Unmanaged,
        LevelScene);
    }
}
