using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Umbraco.Core.Composing.Tests.Testing;

namespace Umbraco.Core.Composing.Tests
{
    [TestFixture]
    public class EnumerableTests : TestsBase
    {
        [TestCaseSource(nameof(Registers))]
        public void CanInjectEmptyEnumerable(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            register.Register<WithEnumerable>();
            var factory = register.CreateFactory();
            var withEnumerable = factory.GetInstance<WithEnumerable>();
            Assert.IsEmpty(withEnumerable.Things);
        }

        [TestCaseSource(nameof(Registers))]
        public void CanResolveEmptyEnumerable(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            var factory = register.CreateFactory();
            var things = factory.GetInstance<IEnumerable<IThing>>();
            Assert.IsEmpty(things);
        }

        [TestCaseSource(nameof(Registers))]
        public void CanInjectEnumerable(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            register.Register<IThing, Thing1>();
            register.Register<IThing, Thing2>();
            register.Register<WithEnumerable>();
            var factory = register.CreateFactory();
            var withEnumerable = factory.GetInstance<WithEnumerable>();
            Assert.AreEqual(2, withEnumerable.Things.Count());
        }

        [TestCaseSource(nameof(Registers))]
        public void CanResolveEnumerable(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            register.Register<IThing, Thing1>();
            register.Register<IThing, Thing2>();
            var factory = register.CreateFactory();
            var things = factory.GetInstance<IEnumerable<IThing>>();
            Assert.AreEqual(2, things.Count());
        }

        [TestCaseSource(nameof(Registers))]
        public void CanResolveAll(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            register.Register<IThing, Thing1>();
            register.Register<IThing, Thing2>();
            register.Register<WithEnumerable>();
            var factory = register.CreateFactory();
            var things = factory.GetAllInstances<IThing>();
            Assert.AreEqual(2, things.Count());
        }

        [TestCaseSource(nameof(Registers))]
        public void CanResolveEnumerableOfSingletons(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            register.Register<IThing, Thing1>(Lifetime.Singleton);
            register.Register<IThing, Thing2>(Lifetime.Singleton);
            var factory = register.CreateFactory();
            var things = factory.GetInstance<IEnumerable<IThing>>();
            Assert.AreEqual(2, things.Count());
        }

        [TestCaseSource(nameof(Registers))]
        public void CanResolveAllOfSingletons(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            register.Register<IThing, Thing1>(Lifetime.Singleton);
            register.Register<IThing, Thing2>(Lifetime.Singleton);
            var factory = register.CreateFactory();
            var things = factory.GetAllInstances<IThing>();
            Assert.AreEqual(2, things.Count());
        }
    }
}