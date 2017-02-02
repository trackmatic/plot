using System;

namespace Plot.Attributes
{
    [AttributeUsage(AttributeTargets.Property|AttributeTargets.Class)]
    public class IgnoreAttribute : Attribute
    {
    }
}
