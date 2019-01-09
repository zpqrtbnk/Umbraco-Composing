using System.Collections.Generic;

namespace Umbraco.Core.Composing.Tests.Testing
{
    public class WithEnumerable
    {
        public WithEnumerable(IEnumerable<IThing> things)
        {
            Things = things;
        }

        public IEnumerable<IThing> Things { get; }
    }
}