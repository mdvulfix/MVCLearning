using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatorController: IController
{
    
    private readonly string m_DisplayNumberFormat = "# ### ### ##0.00";
    private readonly string m_DisplayBlank = "";
    private readonly string m_DisplayBlankSign = "|";
    private bool m_DisplayIsNotEmpty;
    
    private ICalculatorView m_View;
    private ICalculatorModel m_Model;

    private double m_FirstNumber;
    private double m_LastNumber;
    private OperationIndex m_OperationIndex;
    private double m_Result;

    public event Action<string> DisplayUpdateRequired;

    public CalculatorController(ICalculatorView view)
    {
        m_View = view;
        m_Model = new CalculatorDefault();
    }

    public CalculatorController(ICalculatorView view, ICalculatorModel model)
    {
        m_View = view;
        m_Model = model;
    }
    
    public void Setup()
    {
        foreach (var button in m_View.Buttons)
        {
            button.ButtonClicked += OnButtonClicked;
        }

        DisplayClear();
    }
    
    public void Dispose()
    {
        foreach (var button in m_View.Buttons)
        {
            button.ButtonClicked -= OnButtonClicked;
        }

        
    } 


    private void OnButtonClicked(IActionInfo info)
    {
        if (info is NumberInfo)
            SetValue(((NumberInfo)info).Number);
        else if (info is OperationInfo)
            SetOperator(((OperationInfo)info).OperationIndex);
        else if (info is SystemActionInfo)
            Execute(((SystemActionInfo)info).SystemActionIndex);
        else
            throw new Exception("Action info is not defined!");

    }

    private double Calculate(double a, double b, Func<double, double, double> func)
    {
        return m_Model.Calculate(a, b, func);
    }
    
    private void SetValue(double value)
    { 
        DisplayFill(value);
    }

    private void SetOperator(OperationIndex index)
    { 
        SetNumber(ref m_FirstNumber);
        //DisplayClear();
        m_OperationIndex = index;

        switch (m_OperationIndex)
        {
            case OperationIndex.Add:
                //UpdateDisplay("+");
                break;
            case OperationIndex.Substract:
                //UpdateDisplay("-");
                break;
            case OperationIndex.Multiply:
                //UpdateDisplay("x");
                break;
            case OperationIndex.Devide:
                //UpdateDisplay("/");
                break;
            
            default:
                throw new Exception("Operation info is not defined!");
        }
    }

    private void Execute(SystemActionIndex index)
    { 
        SetNumber(ref m_LastNumber);
        

        switch (index) 
        {
            case SystemActionIndex.Calculate:
                
                switch (m_OperationIndex)
                {
                    case OperationIndex.Add:
                        m_Result = Calculate(m_FirstNumber, m_LastNumber, (a, b)=> Math.Add(m_FirstNumber, m_LastNumber));
                        break;
                    case OperationIndex.Substract:
                        m_Result = Calculate(m_FirstNumber, m_LastNumber, (a, b)=> Math.Substract(m_FirstNumber, m_LastNumber));
                        break;
                    case OperationIndex.Multiply:
                        m_Result = Calculate(m_FirstNumber, m_LastNumber, (a, b)=> Math.Multiply(m_FirstNumber, m_LastNumber));
                        break;
                    case OperationIndex.Devide:
                        m_Result = Calculate(m_FirstNumber, m_LastNumber, (a, b)=> Math.Devide(m_FirstNumber, m_LastNumber));
                        break;
                    
                    default:
                        throw new Exception("Operation is not defined!");
                }

                DisplayClear();
                DisplayFill(m_Result);
                Reset();

                break;
            
            case SystemActionIndex.Clear:
                
                DisplayClear();
                Reset();
                break;
            
            default:
                throw new Exception("System action is not defined!");
        
        }
    }

    private void Reset()
    {
        m_FirstNumber = 0;
        m_LastNumber = 0;
        m_OperationIndex = OperationIndex.None;
    }

    private void SetNumber(ref double number)
    {
        number = Convert.ToDouble(m_View.Display.text);
    }
    
    
    
    private void DisplayFill (double value)
    {
        //m_DisplayIsNotEmpty = true;
        //StopCoroutine(DisplayUpdateEmptyValue());
        
        var displayTextValue = m_View.Display.text;
        
        if(displayTextValue == m_DisplayBlank || displayTextValue == m_DisplayBlankSign)
        {
            displayTextValue = value.ToString(m_DisplayNumberFormat);
            DisplayUpdateRequired?.Invoke(displayTextValue);
            return;
        }
        
        
        if(Convert.ToDouble(displayTextValue) == m_FirstNumber || Convert.ToDouble(displayTextValue) == m_LastNumber || Convert.ToDouble(displayTextValue) == m_Result)
        {
            DisplayClear();
            displayTextValue = value.ToString(m_DisplayNumberFormat);
            DisplayUpdateRequired?.Invoke(displayTextValue);
            return;
        }
        
        displayTextValue = (Convert.ToDouble(displayTextValue) * 10 + value).ToString(m_DisplayNumberFormat);
        DisplayUpdateRequired?.Invoke(displayTextValue);
    
    }
    
    private void DisplayClear()
    {
        DisplayUpdateRequired?.Invoke(m_DisplayBlank);
    }



    private IEnumerator DisplayUpdateEmptyValue()
    {
        while(true)
        {
            
            //if(m_DisplayIsNotEmpty)
            //    yield return null;

            //yield return new WaitForSeconds(1);
            //m_Display.text = "|";
            
            //if(m_DisplayIsNotEmpty)
            //    yield return null;

            //yield return new WaitForSeconds(1);
            //m_Display.text = "";

            //if(m_DisplayIsNotEmpty)
            //    yield return null;
        } 
    }

}

public interface IController: IDisposable
{

}

