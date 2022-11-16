using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ButtonMenu : IButton
{
    private Button m_Button;
    private StateIndex m_StateIndex = StateIndex.MenuLoading;

    public ButtonMenu(Button button)
    {
        m_Button = button;
        var info = new StateInfo(m_StateIndex);
        m_Button.onClick.AddListener(() => Clicked?.Invoke(info));
    }

    public event Action<IActionInfo> Clicked;

}