using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class SceneView: View
{
    
    [SerializeField]
    private GameObject m_SceneUI;
    
    public SceneIndex SceneIndex { get; protected set; }
    public event Action<StateIndex> StateRequired;
    
    public void Activate(bool active)
    {
        var index = (int)SceneIndex;
        var scene = SceneManager.GetSceneByBuildIndex(index);
        SceneManager.SetActiveScene(scene);
        m_SceneUI.gameObject.SetActive(active);
    }


    protected void OnButtonClicked(IActionInfo info)
    {
        if (info is StateInfo)
        { 
            var index = ((StateInfo)info).StateIndex;
            StateRequired?.Invoke(index);
        }
        
        
    }

}

public interface ISceneView: IView
{
    SceneIndex SceneIndex { get;  }
    event Action<StateIndex> StateRequired;
    void Activate(bool active);

}