namespace ContainerTests.DemoTypes
{
    using Utils;

    public class Chopper : IFlyingMachine
    {
        public IWindow Window { get; }

        public Chopper(IWindow window)
        {
            Guard.NotNull(window, nameof(window));

            Window = window;
        }
    }
}