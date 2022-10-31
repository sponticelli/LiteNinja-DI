using System;

namespace LiteNinja.DI.Attributes
{
    [AttributeUsage( AttributeTargets.Field )]
    public sealed class InjectAttribute : Attribute
    {
    }
}