using Unity.Entities.Serialization;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SubsceneLoader : MonoBehaviour
{
    public string GameScene = "Reactor";
    public string MenuScene = "GameMenu";

    private Entity _currentLevel;

    public EntitySceneReference LevelScene;

    public static SubsceneLoader Instance;


    private void Awake()
    {
        _currentLevel = Entity.Null;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    public void StartGameAtLevel()
    {
        LoadGameScene();
        //LoadLevel();
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(GameScene, LoadSceneMode.Single);

        //SceneSystem.UnloadScene(World.DefaultGameObjectInjectionWorld.Unmanaged, Scene.SceneGUID,
        //    SceneSystem.UnloadParameters.DestroyMetaEntities);
    }


    public void LoadLevel()
    {
        if (!Entity.Null.Equals(_currentLevel))
        {
            Debug.Log("Unloading");
            SceneSystem.UnloadScene(World.DefaultGameObjectInjectionWorld.Unmanaged,
                _currentLevel, SceneSystem.UnloadParameters.DestroyMetaEntities);
        }

        Debug.Log("Loading Async");
        _currentLevel =
        SceneSystem.LoadSceneAsync(
        World.DefaultGameObjectInjectionWorld.Unmanaged,
        LevelScene);

    }
}
