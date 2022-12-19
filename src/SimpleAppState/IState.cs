namespace SimpleAppState
{
    public interface IState
    {
        event Action? StateChanged;
    }
}