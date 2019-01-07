using System;
using System.Collections.Generic;

namespace Umbraco.Core.Composing.CastleWindsor
{
    public class CastleWindsorContainer : IRegister, IFactory, IDisposable
    {
        /// <summary>
        /// Creates a new instance of the <see cref="CastleWindsorContainer"/> class.
        /// </summary>
        public static CastleWindsorContainer Create()
            => new CastleWindsorContainer();

        public object Concrete { get; }

        public object GetInstance(Type type)
        {
            throw new NotImplementedException();
        }

        public object TryGetInstance(Type type)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            throw new NotImplementedException();
        }

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

        public void Register(Type serviceType, Lifetime lifetime = Lifetime.Transient)
        {
            throw new NotImplementedException();
        }

        public void Register(Type serviceType, Type implementingType, Lifetime lifetime = Lifetime.Transient)
        {
            throw new NotImplementedException();
        }

        public void Register<TService>(Func<IFactory, TService> factory, Lifetime lifetime = Lifetime.Transient)
        {
            throw new NotImplementedException();
        }

        public void RegisterInstance(Type serviceType, object instance)
        {
            throw new NotImplementedException();
        }

        public void RegisterAuto(Type serviceBaseType)
        {
            throw new NotImplementedException();
        }

        public void ConfigureForWeb()
        {
            throw new NotImplementedException();
        }

        public IFactory CreateFactory()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
