

using System;

public interface IButton
{
    event Action<IActionInfo> ButtonClicked;

}
