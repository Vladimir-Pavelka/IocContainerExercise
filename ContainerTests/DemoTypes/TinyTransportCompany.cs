namespace ContainerTests.DemoTypes
{
    using Utils;

    public class TinyTransportCompany : ITransportCompany
    {
        public IWatercraft Watercraft { get; }

        public TinyTransportCompany(IWatercraft watercraft)
        {
            Guard.NotNull(watercraft, nameof(watercraft));

            Watercraft = watercraft;
        }
    }
}