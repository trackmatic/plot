using System;

namespace Octo.Core.Attributes
{
    public class RelationshipAttribute : Attribute
    {
        public bool Reverse { get; set; }

        public string Name { get; set; }
    }
}
