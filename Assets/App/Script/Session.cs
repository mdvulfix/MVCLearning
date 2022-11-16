using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Session : MonoBehaviour
{

    private StateIndex m_State;
    private IScene m_Scene;


    private void Awake()
    {

    }
    

    private void OnEnable() 
    {
        
    }

    private void OnDisable() 
    {
        if(m_Scene != null)
            m_Scene.StateRequired += OnStateRequired;
    }

    private void Start()
    {
        SetState(StateIndex.MenuLoading);
    }

    private void Update()
    {
        
    }


    private void SceneActivate(SceneIndex sceneIndex)
    {
        var index = (int)sceneIndex;
        Scene[] uSceneLoadedAll = SceneManager.GetAllScenes();
        Scene uSceneTarget = default(Scene);

        foreach (var uSceneLoaded in uSceneLoadedAll)
        {
            if(uSceneLoaded.buildIndex == index)
              uSceneTarget = uSceneLoaded;
        } 

        if(uSceneTarget == default(Scene))
            uSceneTarget = SceneManager.GetSceneByBuildIndex(index);

        var rootObjects = uSceneTarget.GetRootGameObjects();

        foreach (var obj in rootObjects)
        {
            if (obj.TryGetComponent<IScene>(out var targetScene))
            {
                if (m_Scene != null)
                { 
                    m_Scene.StateRequired -= OnStateRequired;
                    m_Scene.Activate(false);
                }

                m_Scene = targetScene;
                m_Scene.Activate(true);
                m_Scene.StateRequired += OnStateRequired;
                return;
            }
        }
        
        throw new Exception("Scene view was not found on " + sceneIndex.ToString());
    
    }


    private IEnumerator SceneLoadAsync(SceneIndex sceneIndex, Action callback)
    {
        var index = (int)sceneIndex;

        var loading = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);

        var loadingTime = 5f;
        while (loading.progress < 0.9f)
        {
            yield return new WaitForSeconds(1);
            loadingTime--;

            if(loadingTime < 0)
                throw new Exception("Can't loading scene by index" + sceneIndex.ToString());
        }

        var uScene = SceneManager.GetSceneByBuildIndex(index);
        SceneManager.SetActiveScene(uScene);
        callback.Invoke();

    }

    
    private void SetState(StateIndex state)
    {
        m_State = state;
        OnStateChanged();
    }
    
    private void OnStateChanged()
    {
        try
        {
            switch (m_State)
            {
                case StateIndex.MenuLoading:
                    StopAllCoroutines();
                    StartCoroutine(SceneLoadAsync(SceneIndex.Menu, () => SetState(StateIndex.MenuRun)));
                    break;

                case StateIndex.MenuRun:
                    SceneActivate(SceneIndex.Menu);
                    break;

                case StateIndex.LevelLoading:
                    StopAllCoroutines();
                    StartCoroutine(SceneLoadAsync(SceneIndex.Level, () => SetState(StateIndex.LevelRun)));
                    break;

                case StateIndex.LevelRun:
                    SceneActivate(SceneIndex.Level);
                    break;
                
                default:
                    throw new Exception("State is not implemented!");

            }
        }
        catch (Exception exception)
        {
            Debug.Log(exception.Message);
        }
        finally
        {
            Debug.Log("State was changed! Current state: " + m_State.ToString());
        }


    }
    private void OnStateRequired(StateIndex state)
    {
        SetState(state);
    }

}

public enum StateIndex
{ 
    None,
    MenuLoading,
    MenuRun,
    LevelLoading,
    LevelRun
}

public enum SceneIndex
{ 
    None,
    Menu,
    Level
}