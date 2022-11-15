using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMenu : View
{

    private SceneController m_Controller;

    public override void SetController()
    {
        m_Controller = new SceneController(this);
    }

}
