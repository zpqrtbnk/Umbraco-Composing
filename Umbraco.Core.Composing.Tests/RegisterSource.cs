using System;
using Umbraco.Core.Composing.CastleWindsor;
using Umbraco.Core.Composing.LightInject;
using Umbraco.Core.Composing.MsDi;

namespace Umbraco.Core.Composing.Tests
{
    public class RegisterSource
    {
        public const string LightInject = "lightinject";
        public const string Castle = "castle";
        public const string MsDi = "msdi";

        public static string[] Registers = { LightInject, Castle, MsDi };

        public static IRegister CreateRegister(string name)
        {
            switch (name.ToLower())
            {
                case LightInject:
                    return LightInjectContainer.Create();
                case Castle:
                    return CastleWindsorContainer.Create();
                case MsDi:
                    return MsDiRegister.Create();
                default:
                    throw new ArgumentOutOfRangeException(nameof(name));
            }
        }
    }
}