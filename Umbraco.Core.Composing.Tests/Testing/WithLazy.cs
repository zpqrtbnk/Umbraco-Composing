using System;

namespace Umbraco.Core.Composing.Tests.Testing
{
    public class WithLazy
    {
        private readonly Lazy<IThing> _thing;

        public WithLazy(Lazy<IThing> thing)
        {
            _thing = thing;
        }

        public IThing Thing => _thing.Value;
    }
}