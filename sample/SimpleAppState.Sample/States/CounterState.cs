namespace SimpleAppState.Sample.States
{
    public class CounterState : ICounterState
    {
        private int _count;

        public event Action? StateChanged;
        public int Count
        {
            get => _count;
            set 
            {
                _count = value;
                StateChanged?.Invoke();
            }
        }

        public void IncrementCount()
        {
            Count++;
        }

        public void DecrementCount()
        {
            Count--;
        }
    }

    public interface ICounterState : IState
    {
        int Count { get; set; }
        void IncrementCount();
        void DecrementCount();
    }
}
