using System;

public abstract class SceneView: View
{
    public void Activate(bool active)
    {
        gameObject.SetActive(active);
    }

}

public interface ISceneView
{
    SceneIndex SceneIndex { get;  }
    event Action<StateIndex> StateRequired;
    void Activate(bool active);
}