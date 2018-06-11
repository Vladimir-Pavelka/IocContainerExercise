namespace IocContainer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class Container : IIocContainer
    {
        private readonly IDictionary<Type, Func<object>> _registrations = new Dictionary<Type, Func<object>>();
        private readonly IDictionary<Type, Lazy<object>> _singletonRegistrations = new Dictionary<Type, Lazy<object>>();

        public void Register<TIntf, TConcrete>() where TIntf : class where TConcrete : class, TIntf
            => _registrations[typeof(TIntf)] = ResolveType<TConcrete>;

        public void RegisterSingleton<TIntf, TConcrete>() where TIntf : class where TConcrete : class, TIntf
            => _singletonRegistrations[typeof(TIntf)] = new Lazy<object>(ResolveType<TConcrete>);

        public void RegisterInstance<TIntf, TInstance>(TInstance instance) where TIntf : class where TInstance : class, TIntf
            => _registrations[typeof(TIntf)] = () => instance;

        public TIntf Resolve<TIntf>() where TIntf : class
        {
            var resolvedType = typeof(TIntf);
            var typeFactory = FindFactory(resolvedType);
            return (TIntf)typeFactory();
        }

        private Func<object> FindFactory(Type resolvedType)
        {
            var hasSome = _registrations.TryGetValue(resolvedType, out var dependencyFactory);
            if (hasSome) return dependencyFactory;

            hasSome = _singletonRegistrations.TryGetValue(resolvedType, out var dependency);
            if (hasSome) return () => dependency.Value;

            throw new InvalidOperationException($"Type {resolvedType} is not registered");
        }

        private TConcrete ResolveType<TConcrete>() where TConcrete : class
        {
            var concreteType = typeof(TConcrete);
            var constructors = concreteType.GetConstructors();
            GuardHasSingleConstructor(concreteType, constructors);
            var constructor = constructors.Single();
            var dependencies = constructor.GetParameters();
            var resolvedDependencies = dependencies.Select(d => FindFactory(d.ParameterType)).Select(factory => factory()).ToArray();
            return (TConcrete)constructor.Invoke(resolvedDependencies);
        }

        private static void GuardHasSingleConstructor(Type concreteType, ConstructorInfo[] constructors)
        {
            if (constructors.Length != 1)
                throw new InvalidOperationException($"Type {concreteType} has multiple constructors defined");
        }
    }
}