using System;
using NUnit.Framework;
using Umbraco.Core.Composing.Tests.Testing;

namespace Umbraco.Core.Composing.Tests
{
    [TestFixture]
    public class LazyTests : TestsBase
    {
        [TestCaseSource(nameof(Registers))]
        public void CanInjectLazy(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            register.Register<IThing, Thing>();
            register.Register<WithLazy>();
            var factory = register.CreateFactory();
            var withLazy = factory.GetInstance<WithLazy>();
            var thing = withLazy.Thing;
            Assert.IsNotNull(thing);
        }

        [TestCaseSource(nameof(Registers))]
        public void CanResolveLazy(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            register.Register<IThing, Thing>();
            var factory = register.CreateFactory();
            var lazything = factory.GetInstance<Lazy<IThing>>();
            Assert.IsNotNull(lazything);
            var thing = lazything.Value;
            Assert.IsNotNull(thing);
        }
    }
}