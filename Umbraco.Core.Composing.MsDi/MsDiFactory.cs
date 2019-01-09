using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Umbraco.Core.Composing.MsDi
{
    public class MsDiFactory : IFactory
    {
        public MsDiFactory(IServiceProvider container)
        {
            Container = container;
        }

        protected IServiceProvider Container { get; }

        public object Concrete => Container;

        public object GetInstance(Type type)
            => Container.GetRequiredService(type);

        public TService GetInstanceFor<TService, TTarget>()
        {
            var n = (TargetedService<TService, TTarget>) GetInstance(typeof(TargetedService<TService, TTarget>));
            return n.Service;
        }

        public object TryGetInstance(Type type)
            => Container.GetService(type);

        public IEnumerable<object> GetAllInstances(Type serviceType)
            => Container.GetServices(serviceType);

        public IEnumerable<TService> GetAllInstances<TService>()
            where TService : class
            => Container.GetServices<TService>();

        public void Release(object instance)
        {
            throw new NotImplementedException();
        }

        public IDisposable BeginScope()
        {
            throw new NotImplementedException();
        }

        public void EnablePerWebRequestScope()
        {
            throw new NotImplementedException();
        }
    }
}