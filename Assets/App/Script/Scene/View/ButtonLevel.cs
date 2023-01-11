using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ButtonLevel : IButton
{
    [SerializeField]
    private Button m_Button;
    private StateIndex m_StateIndex = StateIndex.LevelLoading;

    public ButtonLevel(Button button)
    {
        m_Button = button;

        var info = new StateInfo(m_StateIndex);
        m_Button.onClick.AddListener(() => ButtonClicked?.Invoke(info));
    }

    public event Action<IActionInfo> ButtonClicked;
}
