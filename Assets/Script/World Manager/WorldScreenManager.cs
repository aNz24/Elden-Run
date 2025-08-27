using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using System.Collections.Generic;
using System.Collections;


public class WorldScreenManager : NetworkBehaviour
{
    public static WorldScreenManager instance; 

    // LOADED SCENES
    public List<Scene> loadScenes = new List<Scene>();

    // QUED SCENES
    private List<string> quedSceneIDs = new List<string>();
    private int quedScenesToLoad = 0;
    private Coroutine loadingAdditiveSceneCoroutine;

    // LOADING STATUS
    private bool sceneIsLoading = false;
    private bool sceneIsUnLoading = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        NetworkManager.SceneManager.OnSceneEvent += OnSceneEvent;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        NetworkManager.SceneManager.OnSceneEvent -= OnSceneEvent;

        // UNLOAD ALL SCENES

    }

    private void OnSceneEvent(SceneEvent sceneEvent)
    {
        if (!NetworkManager.IsServer)
            return;

        switch (sceneEvent.SceneEventType)
        {
            case SceneEventType.Load:
                sceneIsLoading = true;
                break;
            case SceneEventType.Unload:
                sceneIsUnLoading = true;
                break;
            case SceneEventType.Synchronize:
                break;
            case SceneEventType.ReSynchronize:
                break;
            case SceneEventType.LoadEventCompleted:
                break;
            case SceneEventType.UnloadEventCompleted:
                sceneIsUnLoading = false;
                break;
            case SceneEventType.LoadComplete:
                loadScenes.Add(sceneEvent.Scene); // CALL WHEN THE SCENE IS FINISHED LOADING, ADDS OUR SCENE TO OUR LOADED SCENES LIST

                if(quedScenesToLoad <= 0)
                    quedSceneIDs.Clear();

                for (int i = 0; i < loadScenes.Count; i++)
                {
                    if (!loadScenes[i].isLoaded)
                        loadScenes.RemoveAt(i);
                }

                sceneIsLoading = false;
                break;
            case SceneEventType.UnloadComplete:
                break;
            case SceneEventType.SynchronizeComplete:
                break;
            case SceneEventType.ActiveSceneChanged:
                break;
            case SceneEventType.ObjectSceneChanged:
                break;
            default:
                break;
        }
    }

    // USED TO LOAD OUR MAIN WOLRD SCENE
    public void LoadWorldScene(int buildIndex) { 
    
        // ACTIVATE OUR LOADING SCREEN
        PlayerUIManager.instance.playerUILoadingScreenManager.ActivateLoadingScreen();

        // GET WORLD SCENE , AND LOAD IT
        string worldScene = SceneUtility.GetScenePathByBuildIndex(buildIndex);
        NetworkManager.Singleton.SceneManager.LoadScene(worldScene, LoadSceneMode.Single);

        // 3 LOAD OUR PLAYER SAVE DATE
        PlayerUIManager.instance.localPlayer.LoadGameFromCurrentCharacterData(ref WorldSaveGameManager.instance.currentCharacterData);

    }

    //USED LOAD ADDITIVE SCENE IN OUR MAIN WORLD SCENE
    public void LoadAdditiveScene(string sceneName)
    {
        // MAKE SURE THE SCENE IS NOT ALREADY LOADED
        for (int i = 0; i < loadScenes.Count; i++)
        {
            if (loadScenes[i] == null)
                continue;

            if (loadScenes[i].name == sceneName && loadScenes[i].isLoaded)
                return;
        }

        // LOAD THE SCENE
        var loadSceneStatus = NetworkManager.Singleton.SceneManager.LoadScene(sceneName ,LoadSceneMode.Additive);

        // LOAD EXTRA IN THE SCENE
    }

    public void LoadAdditiveScenes(List<string> scenesToLoad)
    {
        if (!NetworkManager.IsServer)
            return;

        // PASS ALL OF  OUR SCENES TO LOAD TO OUR QUED SCENE LIST
        for (int i = 0; i < scenesToLoad.Count; i++)
        {
            quedSceneIDs.Add(scenesToLoad[i]);
        }

        quedScenesToLoad = quedSceneIDs.Count;

        if(loadingAdditiveSceneCoroutine != null)
            StopCoroutine(loadingAdditiveSceneCoroutine);

        loadingAdditiveSceneCoroutine = StartCoroutine(LoadAdditiveScenesCoroutine());
    }

    // USED TO LOAD MUTIPLE ADDITIVE SCENES AT ONCE WHEN ENTERING NEW AREA
    private IEnumerator LoadAdditiveScenesCoroutine()
    {
        // CHECK TO SEE IF  A SCENE IS CURRENTLY BEING LOADED/UNLOADED AND IF IT IS,WAIT
        for (int i = 0; i < quedSceneIDs.Count; i++)
        {
            while (sceneIsLoading || sceneIsUnLoading)
            {
                yield return null;
            }

            if(quedSceneIDs[i] == null)
                continue;

            // SORT THOURGH A "QUED" LIST OF SCENES AND LOAD THEM ONE BY ONE
            LoadAdditiveScene(quedSceneIDs[i]);
            quedScenesToLoad--;

            yield return new WaitForFixedUpdate();
        }

        quedScenesToLoad = 0;
        loadingAdditiveSceneCoroutine = null;
        yield return null;
    }
    
}