using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalculatorView : View, ICalculatorView
{
    
    [SerializeField] private Text m_Display;
    
    [Header ("Inputs")]
    [SerializeField] private Button m_Button1;
    [SerializeField] private Button m_Button2;
    [SerializeField] private Button m_Button3;
    [SerializeField] private Button m_Button4;
    [SerializeField] private Button m_Button5;
    [SerializeField] private Button m_Button6;
    [SerializeField] private Button m_Button7;
    [SerializeField] private Button m_Button8;
    [SerializeField] private Button m_Button9;
    [SerializeField] private Button m_Button0;
    
    [Header ("Operations")]
    [SerializeField] private Button m_ButtonAdd;
    [SerializeField] private Button m_ButtonSubstract;
    [SerializeField] private Button m_ButtonMultiply;
    [SerializeField] private Button m_ButtonDevide;
    
    [Header ("Actions")]
    [SerializeField] private Button m_ButtonCalculate;
    [SerializeField] private Button m_ButtonClear;

    private CalculatorController m_Controller;
    
    
    
    public Text Display => m_Display;
    public IEnumerable<IButton> Buttons => m_Buttons;
    
    private void Awake ()
    {
        m_Buttons = new List<IButton> (20);

        Bind<NumberInfo> (m_Button1, new NumberInfo (1));
        Bind<NumberInfo> (m_Button2, new NumberInfo (2));
        Bind<NumberInfo> (m_Button3, new NumberInfo (3));
        Bind<NumberInfo> (m_Button4, new NumberInfo (4));
        Bind<NumberInfo> (m_Button5, new NumberInfo (5));
        Bind<NumberInfo> (m_Button6, new NumberInfo (6));
        Bind<NumberInfo> (m_Button7, new NumberInfo (7));
        Bind<NumberInfo> (m_Button8, new NumberInfo (8));
        Bind<NumberInfo> (m_Button9, new NumberInfo (9));
        Bind<NumberInfo> (m_Button0, new NumberInfo (0));

        Bind<OperationInfo> (m_ButtonAdd, new OperationInfo (OperationIndex.Add));
        Bind<OperationInfo> (m_ButtonSubstract, new OperationInfo (OperationIndex.Substract));
        Bind<OperationInfo> (m_ButtonMultiply, new OperationInfo (OperationIndex.Multiply));
        Bind<OperationInfo> (m_ButtonDevide, new OperationInfo (OperationIndex.Devide));

        Bind<SystemActionInfo> (m_ButtonCalculate, new SystemActionInfo (SystemActionIndex.Calculate));
        Bind<SystemActionInfo> (m_ButtonClear, new SystemActionInfo (SystemActionIndex.Clear));

        m_Controller = new CalculatorController (this);
        m_Controller.DisplayUpdateRequired += OnDisplayUpdate;
        m_Controller.Setup();

    
    }

    private void OnDestroy() 
    {
        m_Controller.DisplayUpdateRequired -= OnDisplayUpdate;
        m_Controller.Dispose();

    }

    private void OnDisplayUpdate(string text)
    {
        m_Display.text = text;
    }








}

public interface ICalculatorView
{
    Text Display { get; }
    IEnumerable<IButton> Buttons { get; }
}