using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Session : MonoBehaviour
{

    private StateIndex m_State;
    private ISceneView m_SceneView;
    private Dictionary<int, Scene> m_ScenesLoaded;

    private void Awake()
    {
        m_ScenesLoaded = new Dictionary<int, Scene>();
    }
    

    private void OnEnable() 
    {
        
    }

    private void OnDisable() 
    {
        if(m_SceneView != null)
            m_SceneView.StateRequired += OnStateRequired;
    }

    private void Start()
    {
        SetState(StateIndex.MenuLoading);
    }

    private void Update()
    {
        
    }

    [Obsolete]
    private void SceneActivate(SceneIndex sceneIndex)
    {
        var index = (int)sceneIndex;

        if (m_ScenesLoaded.Count > 0)
        {
            foreach (var scene in m_ScenesLoaded.Values)
            {
                if (scene.buildIndex == index)
                {
                    SceneManager.SetActiveScene(scene);
                    
                    var rootObjects = scene.GetRootGameObjects();
                    foreach (var obj in rootObjects)
                    {
                        if (obj.TryGetComponent<ISceneView>(out var sceneView))
                        {
                            if (m_SceneView != null)
                            { 
                                m_SceneView.StateRequired -= OnStateRequired;
                                m_SceneView.Activate(false);
                            }

                            m_SceneView = sceneView;
                            m_SceneView.Activate(true);
                            m_SceneView.StateRequired += OnStateRequired;
                            return;
                        }
                    }
                }
            }
        }
        
        throw new Exception("Scene view was not found on " + sceneIndex.ToString());
    }


    private void SceneLoad(SceneIndex sceneIndex, Action callback)
    {
        var index = (int)sceneIndex;
        
        if (m_ScenesLoaded.Count > 0)
        {
            foreach (var scene in m_ScenesLoaded.Values)
            {
                if (scene.buildIndex == index)
                {
                    callback.Invoke();
                    return;
                }
            }
        }


        StopCoroutine(SceneLoadAsync(sceneIndex, callback));
        StartCoroutine(SceneLoadAsync(sceneIndex, callback));
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

        var scene = SceneManager.GetSceneByBuildIndex(index);
        m_ScenesLoaded.Add(scene.buildIndex, scene);
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
                    SceneLoad(SceneIndex.Menu, () => SetState(StateIndex.MenuRun));
                    break;

                case StateIndex.MenuRun:
                    SceneActivate(SceneIndex.Menu);
                    break;

                case StateIndex.LevelLoading:
 
                    SceneLoad(SceneIndex.Level, () => SetState(StateIndex.LevelRun));
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