namespace IocContainer
{
    public interface IIocContainer
    {
        void Register<TIntf, TConcrete>() where TConcrete : class, TIntf where TIntf : class;
        void RegisterSingleton<TIntf, TConcrete>() where TConcrete : class, TIntf where TIntf : class;
        void RegisterInstance<TIntf, TInstance>(TInstance instance) where TInstance : class, TIntf where TIntf : class;
        TIntf Resolve<TIntf>() where TIntf : class;
    }
}