public struct ActionInfo<T>: IActionInfo
{
    public ActionInfo(T info)
    {
        Info = info;
    }

    public T Info {get; private set; }
}
