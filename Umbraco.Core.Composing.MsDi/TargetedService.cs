namespace Umbraco.Core.Composing.MsDi
{
    internal class TargetedService<TService, TTarget>
    {
        public TargetedService(TService service)
        {
            Service = service;
        }

        public TService Service { get; }
    }
}