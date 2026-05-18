using Fram3.UI.GlobalState;

namespace Fram3.UI.Storybook.Stories.States
{
    public sealed class CounterCubit : Cubit<int>
    {
        public CounterCubit() : base(0)
        {
        }

        public void Increment() => Emit(State + 1);

        public void Decrement() => Emit(State - 1);

        public void Reset() => Emit(0);
    }
}