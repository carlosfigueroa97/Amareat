using System;
using Autofac;

namespace Amareat.Utils.ServiceLocator
{
    public class AutofacServiceLocator : IServiceLocator
    {
        #region Properties

        private IContainer Container { get; set; }

        private ContainerBuilder ContainerBuilder { get; set; }

        private static AutofacServiceLocator _instance;
        public static AutofacServiceLocator Instance
        {
            get
            {
                return _instance ?? (_instance = new AutofacServiceLocator());
            }
        }

        #endregion

        private AutofacServiceLocator()
        {
            ContainerBuilder = new ContainerBuilder();
        }

        #region Methods

        public T Resolve<T>() => Container.Resolve<T>();

        public void Build()
        {
            if (Container != null)
                return;

            Container = ContainerBuilder.Build();
        }

        public object Resolve(Type type)
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                return scope.Resolve(type);
            }
        }

        public void Register<T>() where T : class => ContainerBuilder.RegisterType<T>();

        public void Register<TInterface, TImplementation>() where TImplementation : TInterface => ContainerBuilder.RegisterType<TImplementation>().As<TInterface>();

        public void RegisterSingle<TInterface, TImplementation>() where TImplementation : class, TInterface => ContainerBuilder.RegisterType<TImplementation>()
            .As<TInterface>().SingleInstance();

        public void RegisterSingleInstance<TInterface, TImplementation>(TImplementation implementation) where TImplementation : class, TInterface => ContainerBuilder.RegisterInstance<TImplementation>(implementation)
            .As<TInterface>().SingleInstance();

        public void Init()
        {
            Build();
        }

        #endregion
    }
}
