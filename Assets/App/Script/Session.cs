using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Session : MonoBehaviour
{


    private StateIndex m_State;
    private ISceneView m_SceneView;
    private Dictionary<int, Scene> m_Scenes;
    private Dictionary<int, ISceneView> m_SceneViews;

    private void Awake()
    {
        m_Scenes = new Dictionary<int, Scene>();
        m_SceneViews = new Dictionary<int, ISceneView>();
    }
    

    private void OnEnable() 
    {
        //View.ViewLoaded += OnViewLoaded;
    }

    private void OnDisable() 
    {
        
        if(m_SceneView != null)
            m_SceneView.StateRequired += OnStateRequired;

       //View.ViewLoaded -= OnViewLoaded;
    }

    private void Start()
    {
        SetState(StateIndex.MenuLoading);
    }

    private void Update()
    {
        
    }


    
    private void SceneActivate(SceneIndex sceneIndex, Action callback)
    {
        StopCoroutine(SceneActivateAsync(sceneIndex, callback));
        StartCoroutine(SceneActivateAsync(sceneIndex, callback));
    }
    
    
    private IEnumerator SceneActivateAsync(SceneIndex sceneIndex, Action callback)
    {
        var index = (int)sceneIndex;
        var activatingTime = 5f;
        
        while (true)
        {
            if(m_SceneViews.TryGetValue(index, out var sceneView))
            {
                if (m_SceneView != null)
                { 
                    m_SceneView.StateRequired -= OnStateRequired;
                    m_SceneView.Activate(false);
                }

                m_SceneView = sceneView;
                m_SceneView.Activate(true);
                m_SceneView.StateRequired += OnStateRequired;
                callback.Invoke();
            }
            
            yield return new WaitForSeconds(1);
            activatingTime--;

            if(activatingTime < 0)
                throw new Exception("Can't activating scene by index" + sceneIndex.ToString());
        }
    }

    private void SceneLoad(SceneIndex sceneIndex, Action callback)
    {
        StopCoroutine(SceneLoadAsync(sceneIndex, callback));
        StartCoroutine(SceneLoadAsync(sceneIndex, callback));
    }


    private IEnumerator SceneLoadAsync(SceneIndex sceneIndex, Action callback)
    {
        var index = (int)sceneIndex;
        Scene scene;

        var sceneNumber = SceneManager.sceneCount;
        for (int i = 0; i < sceneNumber; i++)
        {
            scene = SceneManager.GetSceneAt(i);
            if (scene.buildIndex == index)
            { 
                m_Scenes.Add(scene.buildIndex, scene);
                callback.Invoke();
                yield return null;
            }
        }


        var loading = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);

        var loadingTime = 5f;
        while (loading.progress < 0.9f)
        {
            yield return new WaitForSeconds(1);
            loadingTime--;

            if(loadingTime < 0)
                throw new Exception("Can't loading scene by index" + sceneIndex.ToString());
        }

        scene = SceneManager.GetSceneAt(index);
        m_Scenes.Add(scene.buildIndex, scene);
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
                    SceneLoad(SceneIndex.Menu, () => SetState(StateIndex.MenuActivating));
                    break;
                
                case StateIndex.MenuActivating:
                    SceneActivate(SceneIndex.Menu, () => SetState(StateIndex.MenuRun));
                    break;

                case StateIndex.MenuRun:
                    Debug.Log("Current state " + m_State);
                    break;

                case StateIndex.LevelLoading:
 
                    SceneLoad(SceneIndex.Level, () => SetState(StateIndex.LevelActivating));
                    break;

                case StateIndex.LevelActivating:
                    SceneActivate(SceneIndex.Level, () => SetState(StateIndex.LevelRun));
                    break;

                case StateIndex.LevelRun:
                    Debug.Log("Current state " + m_State);
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

    private void OnViewLoaded(IView view)
    {
        if (view is ISceneView)
        {
            var sceneView = (ISceneView)view;
            var sceneIndex = (int)sceneView.SceneIndex;
            m_SceneViews.Add(sceneIndex, sceneView);
        }
        
    }

    private void OnViewUnloaded(IView view)
    {
        if (view is ISceneView)
        {
            var sceneView = (ISceneView)view;
            var sceneIndex = (int)sceneView.SceneIndex;
            m_SceneViews.Remove(sceneIndex);
        }
    }

}

public enum StateIndex
{ 
    None,
    MenuLoading,
    MenuActivating,
    MenuRun,
    LevelLoading,
    LevelActivating,
    LevelRun
}

public enum SceneIndex
{ 
    None,
    Menu,
    Level
}