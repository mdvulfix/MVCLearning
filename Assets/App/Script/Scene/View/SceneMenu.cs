using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneMenu : SceneView, ISceneView
{

    [SerializeField]
    private Button m_Button;
    
    private IButton m_Level;


    private SceneController m_Controller;

    public SceneIndex SceneIndex => SceneIndex.Menu;

    public event Action<StateIndex> StateRequired;

    public override void SetController()
    {
        m_Controller = new SceneController(this);
    }


    
    private void Awake() 
    {
        
        SetController();

        m_Level = new ButtonLevel(m_Button);

    }


    private void OnEnable() 
    {
        m_Level.Clicked += OnButtonClicked;
    }

    private void OnDisable() 
    {
        m_Level.Clicked -= OnButtonClicked;
    }

    private void OnButtonClicked(IActionInfo info)
    {
        if (info is StateInfo)
        { 
            var index = ((StateInfo)info).StateIndex;
            StateRequired?.Invoke(index);
        }
        
        
    }

}

