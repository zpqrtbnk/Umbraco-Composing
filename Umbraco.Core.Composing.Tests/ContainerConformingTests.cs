using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Umbraco.Core.Composing.Tests.Testing;

namespace Umbraco.Core.Composing.Tests
{
    [TestFixture]
    public class ContainerConformingTests : TestsBase
    {
        [TestCaseSource(nameof(Registers))]
        public void SingletonServiceIsUnique(string registerName) // fixme - but what is LightInject actually doing
        {
            var register = RegisterSource.CreateRegister(registerName);

            // fixme
            // LightInject is 'unique' per serviceType+serviceName
            // but that's not how all containers work
            // and we should not rely on it
            // if we need unique, use RegisterUnique

            // for Core services that ppl may want to redefine in components,
            // it is important to be able to have a unique, singleton implementation,
            // and to redefine it - how it's done at container's level depends
            // on each container

            // redefine the service
            register.Register<IThing, Thing1>(Lifetime.Singleton);
            register.Register<IThing, Thing2>(Lifetime.Singleton);

            var factory = register.CreateFactory();

            var things = factory.GetInstance<IEnumerable<IThing>>();
            Assert.AreEqual(1, things.Count());

            var thing = factory.GetInstance<IThing>();
            Assert.IsInstanceOf<Thing2>(thing);
        }

        [TestCaseSource(nameof(Registers))]
        public void SingletonImplementationIsNotUnique(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);

            // define two implementations
            register.Register<Thing1>(Lifetime.Singleton);
            register.Register<Thing2>(Lifetime.Singleton);

            var factory = register.CreateFactory();

            var things = factory.GetInstance<IEnumerable<IThing>>();
            Assert.AreEqual(2, things.Count());

            Assert.IsNull(factory.TryGetInstance<IThing>());
        }

        [TestCaseSource(nameof(Registers))]
        public void ActualInstanceIsNotUnique(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);

            // define two instances
            register.RegisterInstance(typeof(Thing1), new Thing1());
            register.RegisterInstance(typeof(Thing1), new Thing2());

            var factory = register.CreateFactory();

            var things = factory.GetInstance<IEnumerable<IThing>>();
            //Assert.AreEqual(2, things.Count());
            Assert.AreEqual(1, things.Count()); // well, yes they are unique?

            Assert.IsNull(factory.TryGetInstance<IThing>());
        }

        [TestCaseSource(nameof(Registers))]
        public void InterfaceInstanceIsNotUnique(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);

            // define two instances
            register.RegisterInstance(typeof(IThing), new Thing1());
            register.RegisterInstance(typeof(IThing), new Thing2());

            var factory = register.CreateFactory();

            var things = factory.GetInstance<IEnumerable<IThing>>();
            //Assert.AreEqual(2, things.Count());
            Assert.AreEqual(1, things.Count()); // well, yes they are unique?

            //Assert.IsNull(factory.TryGetInstance<IThing>());
            Assert.IsNotNull(factory.TryGetInstance<IThing>()); // well, what?
        }

        [TestCaseSource(nameof(Registers))]
        public void CanInjectEnumerableOfBase(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);

            register.Register<Thing1>();
            register.Register<Thing2>();
            register.Register<NeedThings>();

            var factory = register.CreateFactory();

            var needThings = factory.GetInstance<NeedThings>();
            Assert.AreEqual(2, needThings.Things.Count());
        }

        [TestCaseSource(nameof(Registers))]
        public void CanGetEnumerableOfBase(string registerName) // NOT SUPPORTED DON'T DO IT
        {
            var register = RegisterSource.CreateRegister(registerName);

            register.Register<Thing1>();
            register.Register<Thing2>();

            var factory = register.CreateFactory();

            var things = factory.GetInstance<IEnumerable<ThingBase>>();
            Assert.AreEqual(2, things.Count());
        }

        [TestCaseSource(nameof(Registers))]
        public void CanTryGetEnumerableOfBase(string registerName) // NOT SUPPORTED DON'T DO IT
        {
            var register = RegisterSource.CreateRegister(registerName);

            register.Register<Thing1>();
            register.Register<Thing2>();

            var factory = register.CreateFactory();

            var things = factory.TryGetInstance<IEnumerable<ThingBase>>();
            Assert.AreEqual(2, things.Count());
        }

        public abstract class ThingBase : IThing { }
        public class Thing1 : ThingBase { }
        public class Thing2 : ThingBase { }

        public class NeedThings
        {
            public NeedThings(IEnumerable<ThingBase> things)
            {
                Things = things;
            }

            public IEnumerable<ThingBase> Things { get; }
        }
    }
}
