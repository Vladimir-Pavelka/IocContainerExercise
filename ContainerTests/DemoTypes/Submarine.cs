namespace ContainerTests.DemoTypes
{
    using Utils;

    public class Submarine : IWatercraft
    {
        public Submarine()
        {
        }

        public Submarine(IWindow window)
        {
            Guard.NotNull(window, nameof(window));
        }
    }
}