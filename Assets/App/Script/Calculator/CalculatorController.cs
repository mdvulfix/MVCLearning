using System;
using System.Collections.Generic;

public class CalculatorController: IController
{
    private ICalculatorView m_View;
    private ICalculatorModel m_Model;

    private string m_FirstNumber;
    private string m_LastNumber;
    private OperationIndex m_OperationIndex;
    private bool m_IsOperationDefined;

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
            SetNumber(((NumberInfo)info).Number);
        else if (info is OperationInfo)
            SetOperator(((OperationInfo)info).OperationIndex);
        else if (info is SystemActionInfo)
            Execute(((SystemActionInfo)info).SystemActionIndex);
        else
            throw new Exception("Action info is not defined!");

    }

    private void SetNumber(string number)
    { 
        if(m_IsOperationDefined == false)
            m_FirstNumber = number;
        else
            m_LastNumber= number;
    }

    private void SetOperator(OperationIndex index)
    { 
        m_OperationIndex = index;
        m_IsOperationDefined = true;
    }

    private void Execute(SystemActionIndex index)
    { 
        if (index == SystemActionIndex.Calculate)
            if(!string.IsNullOrEmpty(m_FirstNumber) && !string.IsNullOrEmpty(m_LastNumber))
                if(m_IsOperationDefined && m_OperationIndex != OperationIndex.None)
                {
                    var firstNumberValue = Convert.ToDouble(m_FirstNumber);
                    var lastNumberValue = Convert.ToDouble(m_LastNumber);

                    switch (m_OperationIndex)
                    {
                        case OperationIndex.Add:
                            Calculate(firstNumberValue, lastNumberValue, (a, b)=> Math.Add(firstNumberValue, lastNumberValue));
                            break;
                        case OperationIndex.Substract:
                            Calculate(firstNumberValue, lastNumberValue, (a, b)=> Math.Substract(firstNumberValue, lastNumberValue));
                            break;
                        case OperationIndex.Multiply:
                            Calculate(firstNumberValue, lastNumberValue, (a, b)=> Math.Multiply(firstNumberValue, lastNumberValue));
                            break;
                        case OperationIndex.Devide:
                            Calculate(firstNumberValue, lastNumberValue, (a, b)=> Math.Devide(firstNumberValue, lastNumberValue));
                            break;
                        
                        default:
                            throw new Exception("Operation info is not defined!");

                    }

                    Clear();
                }
                    
                    
                    
        
        
        



    }

    private void Calculate(double a, double b, Func<double, double, double> func)
    {
        m_Model.Calculate(a, b, func);
    }

    private void Clear()
    {
        m_FirstNumber = null;
        m_LastNumber = null;
        m_OperationIndex = OperationIndex.None;
        m_IsOperationDefined = false;
    }


}

public interface IController: IDisposable
{

}

