

namespace HTBWorldView
{
    public partial class Map
    {
        private struct Pair<T1, T2>
        {
            public readonly T1 First;
            public readonly T2 Second;

            public Pair(T1 first, T2 second)
            {
                this.First = first;
                this.Second = second;
            }
        }
    }

}
