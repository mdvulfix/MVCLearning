using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneLevel : SceneView, ISceneView 
{

    [SerializeField]
    private Button m_ButtonMenu;


    private IButton m_Menu;

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

        SceneIndex = SceneIndex.Level;

        m_Menu = new ButtonMenu(m_ButtonMenu);

    }


    private void OnEnable() 
    {
        //m_Menu.ButtonClicked += OnButtonClicked;
        //Set(this);
        
    }

    private void OnDisable() 
    {
        //Remove(this);
        //m_Menu.ButtonClicked -= OnButtonClicked;
    }


}
