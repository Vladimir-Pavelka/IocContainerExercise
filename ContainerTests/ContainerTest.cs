namespace ContainerTests
{
    using DemoTypes;
    using IocContainer;
    using FluentAssertions;
    using System;
    using Xunit;

    public class ContainerTest
    {
        private readonly Container _testObject;

        public ContainerTest()
        {
            _testObject = new Container();
        }

        [Fact]
        public void GivenRegisteredTypeWithJustDefaultConstructor_WhenResolveInvoked_ThenInstanceSuccesfullyResolved()
        {
            _testObject.Register<IWheel, RubberWheel>();

            var instance = _testObject.Resolve<IWheel>();

            instance.Should().BeOfType<RubberWheel>();
        }

        [Fact]
        public void GivenNotRegisteredType_WhenResolveInvoked_ThenExceptionIsThrown()
        {
            // _testObject.Register<IFlyingMachine, Chopper>();

            Action act = () => _testObject.Resolve<IFlyingMachine>();

            act.Should().Throw<InvalidOperationException>().Which.Message.Should().Contain(nameof(IFlyingMachine)).And.Contain("not registered");
        }

        [Fact]
        public void GivenMultipleConstructorsRegisteredType_WhenResolveInvoked_ThenExceptionIsThrown()
        {
            _testObject.Register<IWatercraft, Submarine>();

            Action act = () => _testObject.Resolve<IWatercraft>();

            act.Should().Throw<InvalidOperationException>().Which.Message.Should().Contain(nameof(Submarine)).And.Contain("multiple constructors");
        }

        [Fact]
        public void GivenRegisteredTypeWithDependency_WhenResolveInvoked_ThenSuccesfullyResolved()
        {
            _testObject.Register<IWindow, GorillaGlassWindow>();
            _testObject.Register<IFlyingMachine, Chopper>();

            var instance = _testObject.Resolve<IFlyingMachine>();

            instance.Should().BeOfType<Chopper>();
            ((Chopper)instance).Window.Should().BeOfType<GorillaGlassWindow>();
        }


        [Fact]
        public void GivenDependencyNotRegistered_WhenResolveInvoked_ThenExceptionIsThrown()
        {
            //_testObject.Register<IWindow, GorillaGlassWindow>();
            _testObject.Register<IFlyingMachine, Chopper>();

            Action act = () => _testObject.Resolve<IFlyingMachine>();

            act.Should().Throw<InvalidOperationException>().Which.Message.Should().Contain(nameof(IWindow)).And.Contain("not registered");
        }

        [Fact]
        public void GivenRegisteredAsInstance_WhenResolvedTwice_ThenItsTheSameInstance()
        {
            var submarine = new Submarine();
            _testObject.RegisterInstance<IWatercraft, Submarine>(submarine);

            var instance = _testObject.Resolve<IWatercraft>();
            var instance2 = _testObject.Resolve<IWatercraft>();

            instance.Should().BeOfType<Submarine>();
            instance.Should().Be(instance2);
        }

        [Fact]
        public void GivenMultipleConstructorsRegisteredAsInstance_WhenDependingTypeResolved_ThenResolveSuccessful()
        {
            var submarine = new Submarine();
            _testObject.RegisterInstance<IWatercraft, Submarine>(submarine);
            _testObject.Register<ITransportCompany, TinyTransportCompany>();

            var instance = _testObject.Resolve<ITransportCompany>();

            instance.Should().BeOfType<TinyTransportCompany>();
            ((TinyTransportCompany)instance).Watercraft.Should().Be(submarine);
        }

        [Fact]
        public void GivenRegisteredAsSingleton_WhenResolvedTwice_ThenItsTheSameInstance()
        {
            _testObject.RegisterSingleton<IWindow, GorillaGlassWindow>();
            _testObject.Register<IFlyingMachine, Chopper>();

            var instance = _testObject.Resolve<IFlyingMachine>();
            var instance2 = _testObject.Resolve<IFlyingMachine>();

            instance.Should().BeOfType<Chopper>();
            instance.Should().NotBe(instance2);
            ((Chopper)instance).Window.Should().BeOfType<GorillaGlassWindow>();
            ((Chopper)instance).Window.Should().Be(((Chopper)instance2).Window);
        }
    }
}