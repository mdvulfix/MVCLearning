using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneMenu : SceneView, ISceneView 
{
    

    
    [SerializeField]
    private Button m_ButtonLevel;

    private IButton m_Level;

    private SceneController m_Controller;

    public void SetController()
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

        SceneIndex = SceneIndex.Menu;

        m_Level = new ButtonLevel(m_ButtonLevel);

    }


    private void OnEnable() 
    {
        //m_Level.ButtonClicked += OnButtonClicked;
        //Set(this);
        
    }

    private void OnDisable() 
    {
        //Remove(this);
        //m_Level.ButtonClicked -= OnButtonClicked;
    }


}
