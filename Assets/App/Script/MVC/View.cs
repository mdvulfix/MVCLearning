using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class View : MonoBehaviour
{

    private static Dictionary<int, IView> Views = new Dictionary<int, IView>(100);

    public static event Action<IView> ViewLoaded;
    public static event Action<IView> ViewUnloaded;



    protected static void Set(IView view)
    {
        try
        {
            var code = view.GetHashCode();
            Views.Add(code, view);
            ViewLoaded?.Invoke(view);
        }
        catch (Exception exception)
        {

            Debug.Log(exception.Message);
        }
    }

    protected static void Remove(IView view)
    { 
        try
        {
            var code = view.GetHashCode();
            Views.Remove(code);
            ViewUnloaded?.Invoke(view);
        }
        catch (Exception exception)
        {
            Debug.Log(exception.Message);
        }

    }

}

public interface IView
{

}