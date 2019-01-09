using System;
using Microsoft.Extensions.DependencyInjection;

namespace Umbraco.Core.Composing.MsDi
{
    public class MsDiRegister : IRegister
    {
        private IFactory _factory;

        private MsDiRegister(IServiceCollection container)
        {
            Container = container;

            Container.AddTransient(typeof(Lazy<>), typeof(Lazier<>));
        }

        public static MsDiRegister Create()
            => new MsDiRegister(new ServiceCollection());

        protected IServiceCollection Container { get; }

        public object Concrete => Container;

        public void Register(Type serviceType, Lifetime lifetime = Lifetime.Transient)
        {
            Container.Add(new ServiceDescriptor(serviceType, serviceType, GetLifetime(lifetime)));
        }

        public void Register(Type serviceType, Type implementingType, Lifetime lifetime = Lifetime.Transient)
        {
            Container.Add(new ServiceDescriptor(serviceType, implementingType, GetLifetime(lifetime)));
        }

        public void RegisterFor<TService, TTarget>(Type implementingType, Lifetime lifetime = Lifetime.Transient)
            where TService : class
        {
            Register(implementingType); // fixme - might capture a transient in a singleton and is this bad?
            Register(f => new TargetedService<TService, TTarget>((TService) f.GetInstance(implementingType)), lifetime);
        }

        public void Register<TService>(Func<IFactory, TService> factory, Lifetime lifetime = Lifetime.Transient)
            where TService : class
        {
            Container.Add(new ServiceDescriptor(typeof(TService), _ => factory(_factory), GetLifetime(lifetime)));
        }

        public void RegisterFor<TService, TTarget>(Func<IFactory, TService> factory, Lifetime lifetime = Lifetime.Transient)
            where TService : class
        {
            Register(f => new TargetedService<TService, TTarget>(factory(f)), lifetime);
        }

        private ServiceLifetime GetLifetime(Lifetime lifetime)
        {
            switch (lifetime)
            {
                case Lifetime.Transient:
                    return ServiceLifetime.Transient;
                case Lifetime.Request:
                    return ServiceLifetime.Scoped;
                case Lifetime.Scope:
                    return ServiceLifetime.Scoped;
                case Lifetime.Singleton:
                    return ServiceLifetime.Singleton;
                default:
                    throw new NotSupportedException($"Lifetime {lifetime} is not supported.");
            }
        }

        public void RegisterInstance(Type serviceType, object instance)
        {
            Container.Add(new ServiceDescriptor(serviceType, instance));
        }

        public void RegisterInstanceFor<TService, TTarget>(TService instance)
            where TService : class
        {
            Register(f => new TargetedService<TService, TTarget>(instance));
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
            if (_factory != null)
                throw new InvalidOperationException("A factory has already been created.");
            return _factory = new MsDiFactory(Container.BuildServiceProvider());
        }

        private class Lazier<T> : Lazy<T>
            where T : class
        {
            public Lazier(IServiceProvider provider)
                : base(provider.GetRequiredService<T>)
            { }
        }
    }
}