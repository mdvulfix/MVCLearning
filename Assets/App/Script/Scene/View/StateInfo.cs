public struct StateInfo: IActionInfo
{
    public StateInfo(StateIndex state)
    {
        StateIndex = state;
    }

    public StateIndex StateIndex {get; private set; }
}
