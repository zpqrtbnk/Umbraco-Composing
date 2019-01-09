using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;

namespace Umbraco.Core.Composing.CastleWindsor
{
    public class CastleWindsorContainer : IRegister, IFactory, IDisposable
    {
        protected CastleWindsorContainer(WindsorContainer container)
        {
            Container = container;

            // support resolving and injecting enumerable of services
            Container.Kernel.Resolver.AddSubResolver(new CollectionResolver(Container.Kernel, true));

            // support resolving and injecting lazy of services
            Container.Register(Component.For<ILazyComponentLoader>().ImplementedBy<LazyOfTComponentLoader>());
        }

        /// <summary>
        /// Creates a new instance of the <see cref="CastleWindsorContainer"/> class.
        /// </summary>
        public static CastleWindsorContainer Create()
            => new CastleWindsorContainer(new WindsorContainer());

        protected WindsorContainer Container { get; }

        public object Concrete => Container;

        private static string GetTargetedServiceName<TTarget>() => "TARGET:" + typeof(TTarget).FullName;

        public object GetInstance(Type type)
        {
            if (Container.Kernel.HasComponent(type) || !type.IsGenericType || type.GetGenericTypeDefinition() != typeof(IEnumerable<>))
                return Container.Resolve(type);
            var enumerableType = type.GenericTypeArguments[0];
            if (Container.Kernel.HasComponent(enumerableType))
                return Container.ResolveAll(enumerableType);
            return Array.CreateInstance(enumerableType, 0);
        }

        public TService GetInstanceFor<TService, TTarget>()
            => Container.Resolve<TService>(GetTargetedServiceName<TTarget>());

        public object TryGetInstance(Type type)
        {
            return Container.Kernel.HasComponent(type) ? GetInstance(type) : null;
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
            => Container.ResolveAll(serviceType).Cast<object>();

        public IEnumerable<TService> GetAllInstances<TService>()
            where TService : class
            => Container.ResolveAll(typeof(TService)).Cast<TService>();

        public void Release(object instance)
            => Container.Release(instance);

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
            Container.Register(Component.For(serviceType).ImplementedBy(serviceType).WithLifetime(lifetime));
        }

        public void Register(Type serviceType, Type implementingType, Lifetime lifetime = Lifetime.Transient)
        {
            Container.Register(Component.For(serviceType).ImplementedBy(implementingType).WithLifetime(lifetime));
        }

        public void RegisterFor<TService, TTarget>(Type implementingType, Lifetime lifetime = Lifetime.Transient)
            where TService : class
        {
            Container.Register(Component.For(typeof(TService)).Named(GetTargetedServiceName<TTarget>()).ImplementedBy(implementingType).WithLifetime(lifetime));
        }

        public void Register<TService>(Func<IFactory, TService> factory, Lifetime lifetime = Lifetime.Transient)
            where TService : class
        {
            Container.Register(Component.For<TService>().UsingFactoryMethod(() => factory(this)).WithLifetime(lifetime));
        }

        public void RegisterFor<TService, TTarget>(Func<IFactory, TService> factory, Lifetime lifetime = Lifetime.Transient)
            where TService : class
        {
            Container.Register(Component.For<TService>().Named(GetTargetedServiceName<TTarget>()).UsingFactoryMethod(() => factory(this)).WithLifetime(lifetime));
        }

        public void RegisterInstance(Type serviceType, object instance)
        {
            Container.Register(Component.For(serviceType).Instance(instance));
        }

        public void RegisterInstanceFor<TService, TTarget>(TService instance)
            where TService : class
        {
            Container.Register(Component.For<TService>().Named(GetTargetedServiceName<TTarget>()).Instance(instance));
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
            return this;
        }

        public void Dispose()
        {
            Container.Dispose();
        }
    }

    internal static class ComponentRegistrationExtensions
    {
        public static ComponentRegistration<T> WithLifetime<T>(this ComponentRegistration<T> registration, Lifetime lifetime)
            where T : class
        {
            switch (lifetime)
            {
                case Lifetime.Transient:
                    return registration.LifestyleTransient();
                case Lifetime.Scope:
                    return registration.LifestyleScoped();
                case Lifetime.Singleton:
                    return registration.LifestyleSingleton();
                default:
                    throw new NotSupportedException(lifetime.ToString());
            }
        }
    }
}
