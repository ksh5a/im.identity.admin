using System;
using IM.Identity.BI.Service.Interface;

namespace IM.Identity.BI.Service.Manager
{
    public class ServiceManager : IDisposable
    {
        private static IEntityService _serviceInstance;

        protected static ServiceUnityFactory ServiceFactory
        {
            get { return new ServiceUnityFactory(); }
        }

        public static IEntityService GetServiceInstance(Type serviceType)
        {
            try
            {
                _serviceInstance = ServiceFactory.GetService(serviceType);

                return _serviceInstance;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while creating a Service instance of type [" + serviceType.FullName + "]", ex);
            }
        }

        public void Dispose()
        {
            _serviceInstance = null;
        }
    }
}