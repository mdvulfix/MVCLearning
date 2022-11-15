using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller
{
    private View m_View;
    private Model m_Model;

    protected void Setup(View view, Model model)
    {
        m_View = view;
        m_Model = model;
    }


}