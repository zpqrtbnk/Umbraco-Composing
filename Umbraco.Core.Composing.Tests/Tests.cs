using NUnit.Framework;
using Umbraco.Core.Composing.LightInject;

namespace Umbraco.Core.Composing.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Test()
        {
            var register = (IRegister) LightInjectContainer.Create();
            register.Register<Foo>();
            var factory = register.CreateFactory();
            var foo = factory.GetInstance<Foo>();
            Assert.IsNotNull(foo);
        }

        public class Foo 
        { }
    }
}
