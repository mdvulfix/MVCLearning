

public abstract class SceneView: View
{
    public void Activate(bool active)
    {
        gameObject.SetActive(active);
    }

}