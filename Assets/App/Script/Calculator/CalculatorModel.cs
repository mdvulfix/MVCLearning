using System;
using UnityEngine.UI;

public class CalculatorDefault : CalculatorModel, ICalculatorModel
{
    public override double Calculate(double a, double b, Func<double, double, double> func)
        => func.Invoke(a, b);

}

public abstract class CalculatorModel
{
    public abstract double Calculate(double a, double b, Func<double, double, double> func);

}


public interface ICalculatorModel
{
    double Calculate(double a, double b, Func<double, double, double> func);
}

public delegate int Operation(int a, int b);

public enum OperationIndex
{ 
    None,
    Add,
    Substract,
    Multiply,
    Devide


}

public enum SystemActionIndex
{ 
    None,
    Calculate,
    Clear,
    Record,
    Reed,

}




public abstract class ButtonModel
{ 
    protected Button m_Button;

    public event Action<IActionInfo> ButtonClicked;

    protected void Clicked(IActionInfo info)
        => ButtonClicked?.Invoke(info);




}


public class Button<TActionInfo>: ButtonModel, IButton
where TActionInfo: IActionInfo
{
    public Button(Button button, TActionInfo info)
    {
        m_Button = button;
        m_Button.onClick.AddListener(() => Clicked(info));
    }

}


public struct NumberInfo: IActionInfo
{
    public NumberInfo(string number)
    {
        Number = number;
    }

    public string Number {get; private set; }
}

public struct OperationInfo: IActionInfo
{
    public OperationInfo(OperationIndex index)
    {
        OperationIndex = index;
    }

    public OperationIndex OperationIndex {get; private set; }
}

public struct SystemActionInfo: IActionInfo
{
    public SystemActionInfo(SystemActionIndex index)
    {
        SystemActionIndex = index;
    }

    public SystemActionIndex SystemActionIndex {get; private set; }
}