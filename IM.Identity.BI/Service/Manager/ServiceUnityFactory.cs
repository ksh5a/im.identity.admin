using System;
using IM.Identity.BI.Service.Interface;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace IM.Identity.BI.Service.Manager
{
    public class ServiceUnityFactory
    {
        private static IUnityContainer _container;

        public ServiceUnityFactory()
        {
            if (_container == null)
            {
                _container = new UnityContainer();
                _container.LoadConfiguration();

                var locator = new UnityServiceLocator(_container);
                ServiceLocator.SetLocatorProvider(() => locator);
            }
        }

        public IEntityService GetService(Type serviceType)
        {
            dynamic repository = ServiceLocator.Current.GetInstance(serviceType);

            return repository;
        }
    }
}