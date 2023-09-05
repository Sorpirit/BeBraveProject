using System;

namespace Scripts.DependancyInjector
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class InjectAttribute : Attribute { }
}
