using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class View : MonoBehaviour
{

    private static Dictionary<int, ISkin> m_Skins = new Dictionary<int, ISkin>(100);

    protected List<IButton> m_Buttons;
    
    public static event Action<ISkin> SkinLoaded;
    public static event Action<ISkin> SkinUnloaded;

    private void SkinSet(ISkin skin)
    {
        try
        {
            var code = skin.GetHashCode();
            m_Skins.Add(code, skin);
            SkinLoaded?.Invoke(skin);
        }
        catch (Exception exception)
        {

            Debug.Log(exception.Message);
        }
    }

    private void SkinRemove(ISkin skin)
    { 
        try
        {
            var code = skin.GetHashCode();
            m_Skins.Remove(code);
            SkinUnloaded?.Invoke(skin);
        }
        catch (Exception exception)
        {
            Debug.Log(exception.Message);
        }

    }

    protected virtual void Bind<TActionInfo> (Button button, TActionInfo info)
    where TActionInfo : IActionInfo
    {
        m_Buttons.Add (new Button<TActionInfo> (button, info));

    }
}

public interface ISkin
{
}

public interface IView
{

}