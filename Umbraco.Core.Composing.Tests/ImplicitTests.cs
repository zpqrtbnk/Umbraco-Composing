using System.Linq;
using NUnit.Framework;
using Umbraco.Core.Composing.Tests.Testing;

namespace Umbraco.Core.Composing.Tests
{
    [TestFixture]
    public class ImplicitTests : TestsBase
    {
        [TestCaseSource(nameof(Registers))]
        [Ignore("No register supports this.")]
        public void CanRegisterAndGetImplicitService(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            register.Register<Thing>();
            var factory = register.CreateFactory();
            var thing = factory.GetInstance<IThing>();
            Assert.IsNotNull(thing);
        }

        [TestCaseSource(nameof(Registers))]
        public void CannotRegisterAndGetImplicitService(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            register.Register<Thing>();
            var factory = register.CreateFactory();
            var thing = factory.TryGetInstance<IThing>();
            Assert.IsNull(thing);
        }

        [TestCaseSource(nameof(Registers))]
        [Ignore("Only LightInject supports this.")] // fixme - but we rely on it!
        public void CanRegisterAndGetAllImplicitService(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            register.Register<Thing>();
            var factory = register.CreateFactory();
            var thing = factory.GetAllInstances<IThing>();
            Assert.IsNotEmpty(thing);
        }

        [TestCaseSource(nameof(Registers))]
        public void CannotRegisterAndGetAllImplicitService(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            register.Register<Thing>();
            var factory = register.CreateFactory();
            var things = factory.GetAllInstances<IThing>();
            Assert.AreEqual(0, things.Count());

            // lightInject: is zero with option EnableVariance set to false
            // others: always zero
        }

        [TestCaseSource(nameof(Registers))]
        [Ignore("Only LightInject supports this.")] // fixme - but we rely on it!
        public void CanRegisterAndInjectImplicitService(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            register.Register<Thing>();
            register.Register<WithEnumerable>();
            var factory = register.CreateFactory();
            var withEnumerable = factory.GetInstance<WithEnumerable>();
            Assert.IsNotEmpty(withEnumerable.Things);
        }
    }
}