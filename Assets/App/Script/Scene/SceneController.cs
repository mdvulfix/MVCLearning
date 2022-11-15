using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : Controller
{
    public SceneController(View view)
    {
        var model = new SceneModel();

        Setup(view, model);
    }
}
