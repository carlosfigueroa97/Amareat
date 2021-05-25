using System;
namespace Amareat.Utils.ServiceLocator
{
    public delegate IServiceLocator ServiceLocatorProviderDelegate();

    public class ServiceLocatorProvider
    {
        private static readonly Lazy<ServiceLocatorProvider> instance = new Lazy<ServiceLocatorProvider>(() =>
        {
            return new ServiceLocatorProvider();
        });

        private ServiceLocatorProvider() { }

        public static ServiceLocatorProvider Instance
        {
            get
            {
                return instance.Value;
            }
        }

        public static ServiceLocatorProviderDelegate ServiceLocatorProviderDelegate = GetAutofacServiceLocator;

        private static IServiceLocator GetAutofacServiceLocator()
        {
            return AutofacServiceLocator.Instance;
        }

        public IServiceLocator Current
        {
            get
            {
                ServiceLocatorProviderDelegate funcImplementation = ServiceLocatorProviderDelegate;
                IServiceLocator implementation = (IServiceLocator)funcImplementation?.Invoke();
                return implementation;
            }
        }

        public TInterface GetService<TInterface>()
        {
            var initializedService = Current.Resolve<TInterface>();
            return initializedService;
        }
    }
}
