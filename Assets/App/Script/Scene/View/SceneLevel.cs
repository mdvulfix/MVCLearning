using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneLevel : SceneView, ISceneView 
{

    

    [SerializeField]
    private Button m_Button;
    private IButton m_Menu;

    private SceneController m_Controller;

    public SceneIndex SceneIndex => SceneIndex.Level;
    
    public event Action<StateIndex> StateRequired;



    public override void SetController()
    {
        m_Controller = new SceneController(this);
    }


    private void OnClick()
    {
        Debug.Log("Level button clicked!");
        
    }
    
    private void Awake() 
    {
        
        SetController();

        m_Menu = new ButtonMenu(m_Button);

    }


    private void OnEnable() 
    {
        m_Menu.Clicked += OnButtonClicked;
    }

    private void OnDisable() 
    {
        m_Menu.Clicked -= OnButtonClicked;
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
