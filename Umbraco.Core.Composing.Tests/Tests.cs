using System.Linq;
using NUnit.Framework;
using Umbraco.Core.Composing.Tests.Testing;

namespace Umbraco.Core.Composing.Tests
{
    [TestFixture]
    public class Tests : TestsBase
    {
        [TestCaseSource(nameof(Registers))]
        public void CanRegisterAndGetTransient(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            register.Register<IThing, Thing>();
            var factory = register.CreateFactory();
            var thing = factory.GetInstance<IThing>();
            Assert.IsNotNull(thing);
            var thing2 = factory.GetInstance<IThing>();
            Assert.IsNotNull(thing2);
            Assert.AreNotSame(thing, thing2);
        }

        [TestCaseSource(nameof(Registers))]
        public void CanRegisterAndGetSingleton(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            register.Register<Thing>(Lifetime.Singleton);
            var factory = register.CreateFactory();
            var thing = factory.GetInstance<Thing>();
            Assert.IsNotNull(thing);
            var thing2 = factory.GetInstance<Thing>();
            Assert.IsNotNull(thing2);
            Assert.AreSame(thing, thing2);
        }

        [TestCaseSource(nameof(Registers))]
        public void CanRegisterAndGetSingletonWithInterface(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            register.Register<IThing, Thing>(Lifetime.Singleton);
            var factory = register.CreateFactory();
            var thing = factory.GetInstance<IThing>();
            Assert.IsNotNull(thing);
            var thing2 = factory.GetInstance<IThing>();
            Assert.IsNotNull(thing2);
            Assert.AreSame(thing, thing2);
        }

        [TestCaseSource(nameof(Registers))]
        public void CanRegisterAndGetSingletonWithFactory(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            register.Register<IThing>(f => new Thing(), Lifetime.Singleton);
            var factory = register.CreateFactory();
            var thing = factory.GetInstance<IThing>();
            Assert.IsNotNull(thing);
            var thing2 = factory.GetInstance<IThing>();
            Assert.IsNotNull(thing2);
            Assert.AreSame(thing, thing2);
        }

        [TestCaseSource(nameof(Registers))]
        public void CanRegisterAndGetInstance(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            var instance = new Thing();
            register.RegisterInstance<IThing>(instance);
            var factory = register.CreateFactory();
            var thing = factory.GetInstance<IThing>();
            Assert.IsNotNull(thing);
            Assert.AreSame(instance, thing);
            var thing2 = factory.GetInstance<IThing>();
            Assert.IsNotNull(thing2);
            Assert.AreSame(thing, thing2);
        }

        [TestCaseSource(nameof(Registers))]
        [Ignore("Fails for Castle. We should not rely on this, and use Unique instead.")]
        public void CanReRegisterAndGetInstance(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            var instance = new Thing();
            register.RegisterInstance<IThing>(instance);
            var instance2 = new Thing();
            register.RegisterInstance<IThing>(instance2); // Castle throws here
            var factory = register.CreateFactory();
            var thing = factory.GetInstance<IThing>();
            Assert.IsNotNull(thing);
            Assert.AreSame(instance2, thing); // what we get is unspecified here
            var thing2 = factory.GetInstance<IThing>();
            Assert.IsNotNull(thing2);
            Assert.AreSame(thing, thing2);
        }

        [TestCaseSource(nameof(Registers))]
        [Ignore("Should not do this.")]
        public void CanRegisterManySingletonsAndGetOne(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            register.Register<IThing, Thing1>(Lifetime.Singleton);
            register.Register<IThing, Thing2>(Lifetime.Singleton);
            var factory = register.CreateFactory();
            var thing = factory.GetInstance<IThing>(); // what we get is unspecified here
            Assert.IsNotNull(thing);
            Assert.IsInstanceOf<Thing1>(thing);
            var thing2 = factory.GetInstance<IThing>();
            Assert.IsNotNull(thing2);
            Assert.AreSame(thing, thing2);

            var things = factory.GetAllInstances<IThing>();
            Assert.AreEqual(2, things.Count());
        }

        [TestCaseSource(nameof(Registers))]
        public void CanRegisterManySingletonsAndGetAll(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            register.Register<IThing, Thing1>(Lifetime.Singleton);
            register.Register<IThing, Thing2>(Lifetime.Singleton);
            var factory = register.CreateFactory();
            var things = factory.GetAllInstances<IThing>();
            Assert.AreEqual(2, things.Count());
        }

        [TestCaseSource(nameof(Registers))]
        public void CannotGetUnregistered(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            var factory = register.CreateFactory();
            try
            {
                var thing = factory.GetInstance<IThing>();
                Assert.Fail("Expected an exception.");
            }
            catch { /* expected */ }
        }

        [TestCaseSource(nameof(Registers))]
        public void CanTryGetRegistered(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            register.Register<IThing, Thing>();
            var factory = register.CreateFactory();
            var thing = factory.TryGetInstance<IThing>();
            Assert.IsNotNull(thing);
        }

        [TestCaseSource(nameof(Registers))]
        public void CanTryGetUnregistered(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            var factory = register.CreateFactory();
            var thing = factory.TryGetInstance<IThing>();
            Assert.IsNull(thing);
        }
    }
}
