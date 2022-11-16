using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneModel : Model
{




}

public interface IScene
{
    event Action<StateIndex> StateRequired;
    void Activate(bool active);
}