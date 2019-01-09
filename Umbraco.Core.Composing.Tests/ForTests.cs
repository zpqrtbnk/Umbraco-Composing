using NUnit.Framework;
using Umbraco.Core.Composing.Tests.Testing;

namespace Umbraco.Core.Composing.Tests
{
    [TestFixture]
    public class ForTests : TestsBase
    {
        [TestCaseSource(nameof(Registers))]
        public void CanGetTransientInstanceFor(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            register.RegisterFor<IThing, int>(_ => new Thing1());
            register.RegisterFor<IThing, string>(_ => new Thing2());
            var factory = register.CreateFactory();
            var thingForInt = factory.GetInstanceFor<IThing, int>();
            var thingForString = factory.GetInstanceFor<IThing, string>();

            Assert.IsInstanceOf<Thing1>(thingForInt);
            Assert.IsInstanceOf<Thing2>(thingForString);

            var thingForInt2 = factory.GetInstanceFor<IThing, int>();
            var thingForString2 = factory.GetInstanceFor<IThing, string>();

            Assert.AreNotSame(thingForInt, thingForInt2);
            Assert.AreNotSame(thingForString, thingForString2);
        }

        [TestCaseSource(nameof(Registers))]
        public void CanGetSingletonInstanceFor(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            register.RegisterFor<IThing, int>(_ => new Thing1(), Lifetime.Singleton);
            register.RegisterFor<IThing, string>(_ => new Thing2(), Lifetime.Singleton);
            var factory = register.CreateFactory();
            var thingForInt = factory.GetInstanceFor<IThing, int>();
            var thingForString = factory.GetInstanceFor<IThing, string>();

            Assert.IsInstanceOf<Thing1>(thingForInt);
            Assert.IsInstanceOf<Thing2>(thingForString);

            var thingForInt2 = factory.GetInstanceFor<IThing, int>();
            var thingForString2 = factory.GetInstanceFor<IThing, string>();

            Assert.AreSame(thingForInt, thingForInt2);
            Assert.AreSame(thingForString, thingForString2);
        }

        [TestCaseSource(nameof(Registers))]
        public void CanGetInstanceFor(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            register.RegisterInstanceFor<IThing, int>(new Thing1());
            register.RegisterInstanceFor<IThing, string>(new Thing2());
            var factory = register.CreateFactory();
            var thingForInt = factory.GetInstanceFor<IThing, int>();
            var thingForString = factory.GetInstanceFor<IThing, string>();

            Assert.IsInstanceOf<Thing1>(thingForInt);
            Assert.IsInstanceOf<Thing2>(thingForString);

            var thingForInt2 = factory.GetInstanceFor<IThing, int>();
            var thingForString2 = factory.GetInstanceFor<IThing, string>();

            Assert.AreSame(thingForInt, thingForInt2);
            Assert.AreSame(thingForString, thingForString2);
        }

        [TestCaseSource(nameof(Registers))]
        public void CanInjectAll(string registerName)
        {
            var register = RegisterSource.CreateRegister(registerName);
            register.RegisterFor<IThing, int>(_ => new Thing1());
            register.RegisterFor<IThing, string>(_ => new Thing2());
            register.Register(f => new Things(f), Lifetime.Singleton);
            var factory = register.CreateFactory();

            // this can be injected
            var things = factory.GetInstance<Things>();

            // no need to use the factory
            var thingForInt = things.For<int>();
            var thingForString = things.For<string>();

            Assert.IsInstanceOf<Thing1>(thingForInt);
            Assert.IsInstanceOf<Thing2>(thingForString);
        }

        private class Things : TargetedServiceProvider<IThing>
        {
            public Things(IFactory factory)
                : base(factory)
            { }
        }
    }
}
